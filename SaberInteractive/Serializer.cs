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
        private static readonly Func<string, string> _packagingItem = item => $"[{item}]";

        private static readonly Func<string, string, string> _packagingProperty =
            (name, value) => string.IsNullOrEmpty(value) ? string.Empty :
                $"{name}:{value}|";

        public static void Serialize(FileStream fileStream, ListNode head)
        {
            var buffer = new StringBuilder();
            var current = head;
            using (StreamWriter writer = new StreamWriter(fileStream))
            {
                while (current != null)
                {
                    var currentBuffer = new StringBuilder();
                    foreach (var fieldInfo in current.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
                    {
                        var fieldvalue = fieldInfo.GetValue(current);
                        if (fieldvalue == null || fieldvalue is Guid)
                            continue;

                        var data = String.Empty;
                        if (fieldvalue is ListNode)
                            data = GetNodeId(fieldvalue as ListNode);
                        else
                        {
                            data = fieldvalue.ToString();
                        }

                        currentBuffer.Append(_packagingProperty(fieldInfo.Name, data));
                    }
                    buffer.Append(_packagingItem(currentBuffer.ToString()));
                    current = current.Next;
                }
                writer.Write(buffer.ToString());
            }
        }

        public static ListNode Deserialize(FileStream s)
        {
            throw new NotImplementedException();
        }

        private static string GetNodeId(ListNode node)
        {
            _nodeDictionary = _nodeDictionary ?? new Dictionary<ListNode, string>();
            if (!_nodeDictionary.ContainsKey(node))
            {
                _nodeDictionary[node] = node.Guid.ToString();
            }
            return _nodeDictionary[node];
        }
    }
}