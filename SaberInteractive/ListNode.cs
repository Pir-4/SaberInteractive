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
                    this.Data.Equals(inputNode.Data) &&
                    EqualsRefNode(this.Perv, inputNode.Perv) &&
                    EqualsRefNode(this.Next, inputNode.Next) &&
                    EqualsRefNode(this.Rand, inputNode.Rand));
            /*ReferenceEquals(this.Perv, inputNode.Perv) && 
             ReferenceEquals(this.Next, inputNode.Next) &&
            ReferenceEquals(this.Rand, inputNode.Rand));*/
        }

        public override int GetHashCode()
        {
            var hash = Data.GetHashCode();

            hash = GetHashCode(hash, this.Perv);
            hash = GetHashCode(hash, this.Next);
            hash = GetHashCode(hash, this.Rand);

            return hash;
        }

        private int GetHashCode(int hash, ListNode node)
        {
            if (node != null)
                hash = hash ^ node.Guid.GetHashCode();
            return hash;
        }

        private bool EqualsRefNode(ListNode expectedField, ListNode inputField)
        {
            if (expectedField == null)
                return inputField == null;
            else if(inputField == null)
            {
                return false;
            }

            // return ReferenceEquals(expectedField, inputField);
             return expectedField.Guid.Equals(inputField.Guid);

        }
    }
}