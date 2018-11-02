using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;


namespace SaberInteractive
{
    public class Serializer
    {
        private static Dictionary<ListNode, string> _nodeDictionary;
        private static readonly Func<string, string> _packagingItem = item => $"[{item}]\n";

        private static readonly Func<string, string, string> _packagingProperty =
            (name, value) => string.IsNullOrEmpty(value) ? string.Empty :
                "{" + $"{name}:{value}" + "}";

        public static void Serialize(FileStream fileStream, ListNode head)
        {
            var buffer = new StringBuilder();
            var current = head;
            using (StreamWriter writer = new StreamWriter(fileStream))
            {
                while (current != null)
                {
                    var currentBuffer = new List<string>();
                    foreach (var fieldInfo in current.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
                    {
                        var fieldValue = fieldInfo.GetValue(current);
                        if (fieldValue == null)
                            continue;

                        var data = string.Empty;
                        if (fieldValue is ListNode)
                            data = (fieldValue as ListNode).Guid.ToString();
                        else
                        {
                            data = fieldValue.ToString();
                        }

                        currentBuffer.Add(_packagingProperty(fieldInfo.Name, data));
                    }
                    buffer.Append(_packagingItem(string.Join(" ",currentBuffer)));
                    current = current.Next;
                }
                writer.Write(buffer.ToString().TrimEnd(Environment.NewLine.ToCharArray()));
            }
        }

        public static ListNode Deserialize(FileStream s)
        {
            throw new NotImplementedException();
        }
    }
}