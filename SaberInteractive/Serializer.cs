using System;
using System.IO;

namespace SaberInteractive
{
    public class Serializer
    {
        private readonly Func<string, string> _packagingItem = item => $"[{item}]";

        private readonly Func<string,string, string> _packagingProperty =
            (name, value) => string.IsNullOrEmpty(value) ? string.Empty :
                $"{name}:{value}";

        public void Serialize(FileStream s, ListNode startNode)
        {
            throw new NotImplementedException();
        }

        public ListNode Deserialize(FileStream s)
        {
            throw new NotImplementedException();
        }
    }
}