using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaberInteractive
{
    public class ListRand
    {
        private ListNode Head;
        private ListNode Tail;

        public int Count { get; private set; }

        public ListNode Add(string data, ListNode randNode = null)
        {
            var newNode = new ListNode() { Data = data, Rand = randNode};
            Count++;

            if (Head == null)
            {
                Head = newNode;
                Tail = newNode;
            }
            else if (Head == Tail)
            {
                Head.Next = newNode;
                newNode.Perv = Head;
                Tail = newNode;
            }
            else
            {
                newNode.Perv = Tail;
                Tail.Next = newNode;
                Tail = newNode;
            }
            return newNode;
        }

        public List<string> ToList(bool isReverse = false)
        {
            var result = new List<string>();

            var current = isReverse ? Tail : Head;
            while (current != null)
            {
                result.Add(current.Data);
                current = isReverse ? current.Perv : current.Next;
            }

            return result;
        }

        public void Serialize(FileStream s)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(FileStream s)
        {
            throw new NotImplementedException();
        }
    }
}
