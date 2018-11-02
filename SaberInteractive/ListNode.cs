using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaberInteractive
{
    public class ListNode
    {
        public string Data;
        public ListNode Perv;
        public ListNode Next;
        public ListNode Rand;
        public readonly Guid Guid = Guid.NewGuid();

        public ListNode(string data)
        {
            Data = data;
        }

        public override bool Equals(object obj)
        {
            var inputNode = obj as ListNode;

            return (inputNode != null) &&
                (ReferenceEquals(this, obj) ||
                 (this.Data.Equals(inputNode.Data) &&
                 ReferenceEquals(this.Perv, inputNode.Perv) && ReferenceEquals(this.Next, inputNode.Next) &&
                 ReferenceEquals(this.Rand, inputNode.Rand)));
        }

        public override int GetHashCode()
        {
            var hash = Data.GetHashCode();

            if (Perv != null)
                hash = hash ^ Perv.Guid.GetHashCode();

            if (Next != null)
                hash = hash ^ Next.Guid.GetHashCode();

            if (Rand != null)
                hash = hash ^ Rand.Guid.GetHashCode();

            return hash;
        }
    }
}