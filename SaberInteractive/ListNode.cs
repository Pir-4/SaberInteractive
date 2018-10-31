using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaberInteractive
{
    public class ListNode
    {
        public ListNode Perv;
        public ListNode Next;
        public ListNode Rand;
        public string Data;

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
            return base.GetHashCode();
        }
    }
}