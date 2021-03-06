﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;


namespace SaberInteractive
{
    public class Serializer
    {
        private const string ItemOpen = "[";
        private const string ItemClose = "]";
        private static readonly string ItemRegexPattern = $@"(^|[\W])\{ItemOpen}(.*?)\{ItemClose}($|(?![ \w\S]))";

        private const string FieldOpen = "{";
        private const string FieldClose = "}";
        private static readonly string FieldRegexPattern = $@"(^|\s|)\{FieldOpen}(.*?)\{FieldClose}($|\s)";

        private const string DataSplitter = ":";
        private static readonly string DataRegexPattern = $"(.*?){DataSplitter}(.*)";

        public static readonly List<string> FailSymbols = new List<string>() { $"{FieldClose} " };

        private static readonly Func<string, string> PackagingItem = item => $"{ItemOpen}" + $"{item}" + $"{ItemClose}\n";

        private static readonly Func<string, string, string> PackagingField =
            (name, value) => $"{FieldOpen}" + $"{name}:{value}" + $"{FieldClose}";

        private static readonly Dictionary<Type, IEnumerable<FieldInfo>> Cache = new Dictionary<Type, IEnumerable<FieldInfo>>();

        public static void Serialize(FileStream fileStream, ListNode node, Func<ListNode, ListNode> next)
        {
            var buffer = new StringBuilder();
            var current = node;
            using (StreamWriter writer = new StreamWriter(fileStream))
            {
                while (current != null)
                {
                    var itemBuffer = new List<string>();
                    foreach (var fieldInfo in FieldInfo(current))
                    {
                        var fieldValue = fieldInfo.GetValue(current);
                        if (fieldValue == null)
                            continue;

                        var data = fieldValue.ToString();
                        if (fieldValue is ListNode)
                            data = (fieldValue as ListNode).Guid.ToString();

                        foreach (var symbol in FailSymbols)
                        {
                            if (data.Contains(symbol))
                                throw new FormatException($"String '{data}' contains bad symbol '{symbol}'. " +
                                                          $"String not contains symbols: {string.Join(", ", FailSymbols)}");
                        }
                        itemBuffer.Add(PackagingField(fieldInfo.Name, data));
                    }
                    buffer.Append(PackagingItem(string.Join(" ", itemBuffer)));
                    current = next(current);
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
                    var elementString = elementMatch.Groups[2].Value;
                    var elementDictionary = new Dictionary<string, string>();

                    foreach (Match fieldMatch in Regex.Matches(elementString, FieldRegexPattern))
                    {
                        var fieldString = fieldMatch.Groups[2].Value;
                        var match = Regex.Match(fieldString, DataRegexPattern);
                        elementDictionary[match.Groups[1].Value] = match.Groups[2].Value;
                    }
                    elementsGuidStringFields[Guid.Parse(elementDictionary[nameof(ListNode.Guid)])] = elementDictionary;
                    elementDictionary.Remove(nameof(ListNode.Guid));
                }
            }

            return ConvertStringToListNode(elementsGuidStringFields);
        }

        private static List<ListNode> ConvertStringToListNode(Dictionary<Guid, Dictionary<string, string>> elementsGuidStringFields)
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
                    SetValueByFieldInfo(currentItem, elementField.Key, insertItem);
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

        private static void SetValueByFieldInfo(object obj, string fieldName, object value)
        {
            FieldInfo(obj).First(info => info.Name.Equals(fieldName)).SetValue(obj, value);
        }
    }
}