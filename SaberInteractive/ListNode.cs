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
        public Guid Guid;

        public ListNode(string data, Guid? guid = null)
        {
            Data = data;
            Guid = guid ?? Guid.NewGuid();
        }

        public override bool Equals(object obj)
        {
            return (obj is ListNode inputNode) &&
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