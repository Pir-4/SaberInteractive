using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;


namespace SaberInteractive
{
    public class Serializer
    {
        private const string ItemOpen = "[";
        private const string ItemClose = "]";
        private static string ItemRegexPattern = $@"\{ItemOpen}(.*?)\{ItemClose}";

        private const string FieldOpen = "{";
        private const string FieldClose = "}";
        private static string FieldRegexPattern = $@"\{FieldOpen}(.*?)\{FieldClose}";

        private const string DataSplitter = ":";
        private static string DataRegexPattern = $"(.*){DataSplitter}(.*)";

        private static Dictionary<Type, IEnumerable<FieldInfo>> Cache = new Dictionary<Type, IEnumerable<FieldInfo>>();


        private static Dictionary<ListNode, string> _nodeDictionary;
        private static readonly Func<string, string> _packagingItem = item => $"{ItemOpen}" +$"{item}" + $"{ItemClose}\n";

        private static readonly Func<string, string, string> _packagingProperty =
            (name, value) => $"{FieldOpen}" + $"{name}:{value}" + $"{FieldClose}";

        public static void Serialize(FileStream fileStream, ListNode head)
        {
            var buffer = new StringBuilder();
            var current = head;
            using (StreamWriter writer = new StreamWriter(fileStream))
            {
                while (current != null)
                {
                    var currentBuffer = new List<string>();
                    foreach (var fieldInfo in FieldInfo(current))
                    {
                        var fieldValue = fieldInfo.GetValue(current);
                        if (fieldValue == null)
                            continue;

                        var data = fieldValue.ToString();
                        if (fieldValue is ListNode)
                            data = (fieldValue as ListNode).Guid.ToString();

                        data = data;//.Replace("[", @"\[").Replace("]", @"\]");
                        currentBuffer.Add(_packagingProperty(fieldInfo.Name, data));
                    }
                    buffer.Append(_packagingItem(string.Join(" ", currentBuffer)));
                    current = current.Next;
                }
                writer.Write(buffer.ToString().TrimEnd(Environment.NewLine.ToCharArray()));
            }
        }

        public static List<ListNode> Deserialize(FileStream fileStream)
        {
            var elementsGuidStringFields = new Dictionary<Guid, Dictionary<string, string>>();

            using (StreamReader reader = new StreamReader(fileStream))
            {
                foreach (Match elementMatch in Regex.Matches(reader.ReadToEnd(), ItemRegexPattern))
                {
                    var elementString = elementMatch.Groups[1].Value;
                    var elementDictionary = new Dictionary<string, string>();

                    foreach (Match fieldMatch in Regex.Matches(elementString, FieldRegexPattern))
                    {
                        var fieldString = fieldMatch.Groups[1].Value;
                        var match = Regex.Match(fieldString, DataRegexPattern);
                        elementDictionary[match.Groups[1].Value] = match.Groups[2].Value;
                    }
                    elementsGuidStringFields[Guid.Parse(elementDictionary[nameof(ListNode.Guid)])] = elementDictionary;
                    elementDictionary.Remove(nameof(ListNode.Guid));
                }
            }

            return Parse(elementsGuidStringFields);
        }

        private static List<ListNode> Parse(Dictionary<Guid, Dictionary<string, string>> elementsGuidStringFields)
        {
            var result = new List<ListNode>();
            var elementsGuidListNode = new Dictionary<Guid, ListNode>();
            foreach (var kvp in elementsGuidStringFields)
            {
                var data = kvp.Value[nameof(ListNode.Data)];
                elementsGuidListNode[kvp.Key] = new ListNode(data, kvp.Key);
                kvp.Value.Remove(nameof(ListNode.Data));
            }

            foreach (var kvp in elementsGuidStringFields)
            {
                var currentItem = elementsGuidListNode[kvp.Key];
                foreach (var elementField in kvp.Value)
                {
                    var guid = Guid.Parse(elementField.Value);
                    var insertItem = elementsGuidListNode[guid];
                    currentItem.GetType().GetField(elementField.Key).SetValue(currentItem, insertItem);
                }
                result.Add(currentItem);
            }
            return result;
        }

        private static FieldInfo[] FieldInfo(object obj)
        {
            var type = obj.GetType();
            if (!Cache.ContainsKey(type))
            {
                Cache[type] = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
            }

            return Cache[type].ToArray();
        }
    }
}