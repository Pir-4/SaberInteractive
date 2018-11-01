using System;
using System.Collections.Generic;
using System.IO;
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
                $"{name}:{value}";

        public static void Serialize(FileStream s, ListNode head)
        {
            var buffer = new StringBuilder();
            var current = head;
            while (current != null)
            {
                var currentBuffer = new StringBuilder();
                foreach (PropertyInfo propertyInfo in current.GetType().GetProperties())
                {
                    var guid = GetNodeId(propertyInfo.GetValue(propertyInfo) as ListNode);
                    currentBuffer.Append(_packagingProperty(propertyInfo.Name, guid));
                }
                buffer.Append(_packagingItem(currentBuffer.ToString()));
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