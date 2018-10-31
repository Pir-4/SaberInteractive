using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace SaberInteractive
{
    public class Serializer
    {
        private readonly Func<string, string> _packagingItem = item => $"[{item}]";

        private readonly Func<string,string, string> _packagingProperty =
            (name, value) => string.IsNullOrEmpty(value) ? string.Empty :
                $"{name}:{value}";

        private Dictionary<ListNode, string> _nodeDictionary;

        public void Serialize(FileStream s, ListNode head)
        {
            var buffer = new StringBuilder();
            _nodeDictionary = new Dictionary<ListNode, string>();
            var current = head;
            while (current != null)
            {
                
            }
        }

        public ListNode Deserialize(FileStream s)
        {
            throw new NotImplementedException();
        }
    }
}