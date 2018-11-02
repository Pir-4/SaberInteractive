using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SaberInteractive
{
    public class ListRand
    {
        public ListNode Head;
        private ListNode Tail;

        public int Count { get; private set; }

        public ListNode Add(string data, ListNode randNode = null)
        {
            return Add(new ListNode(data) {Rand = randNode});
        }

        public ListNode Add(ListNode node)
        {
            Count++;

            if (Head == null)
            {
                Head = node;
            }
            else if (Head == Tail)
            {
                Head.Next = node;
                node.Perv = Head;
            }
            else
            {
                node.Perv = Tail;
                Tail.Next = node;
            }
            Tail = node;
            return node;
        }


        public ListNode GetNode(int index, bool isReverse = false)
        {
            if (index < 0 || index > Count)
            {
                throw new ArgumentException($"Input index not correct. Index from 0 to {Count-1}");
            }

            var current = isReverse ? Tail : Head;
            var count = 0;
            while (count != index && current != null)
            {
                current = isReverse ? current.Perv : current.Next;
                count++;
            }

            return current;
        }

        public List<string> ToListString(bool isReverse = false)
        {
            var result = new List<string>();
            ToList(current => result.Add(current.Data), isReverse);
            return result;
        }

        public List<ListNode> ToListNode(bool isReverse = false)
        {
            var result = new List<ListNode>();
            ToList(current => result.Add(current), isReverse);
            return result;
        }

        public bool IsEmpty
        {
            get { return Head == null; }
        }

        public void Serialize(FileStream fileStream)
        {
            Serializer.Serialize(fileStream, Head);
        }

        public void Deserialize(FileStream fileStream)
        {
            var tempList = Serializer.Deserialize(fileStream);
            InitHeads(tempList);
        }

        public override bool Equals(object obj)
        {
            return (obj is ListRand inputListRand) &&
                   (ReferenceEquals(this, obj) || EqualsListRand(inputListRand));
        }

        private void ToList(Action<ListNode> actionCurrentItem, bool isReverse = false)
        {
            var current = isReverse ? Tail : Head;
            while (current != null)
            {
                actionCurrentItem.Invoke(current);
                current = isReverse ? current.Perv : current.Next;
            }
        }

        private void InitHeads(List<ListNode> inputList)
        {
            var headsCount = 0;
            var tailsCount = 0;
            foreach (var node in inputList)
            {
                if (node.Perv == null)
                {
                    Head = node;
                    headsCount++;
                }

                if (node.Next == null)
                {
                    Tail = node;
                    tailsCount++;
                }

                if (headsCount > 1)
                {
                    throw new ArgumentException($"Input list has more one heads. Has {headsCount}");
                }
                if (tailsCount > 1)
                {
                    throw new ArgumentException($"Input list has more one tails. Has {tailsCount}");
                }
            }

            Count = inputList.Count;
        }

        private bool EqualsListRand(ListRand inputListRand)
        {
            if (!inputListRand.Count.Equals(this.Count))
                return false;

            var inputCurrent = inputListRand.GetNode(0);
            var current = Head;

            if (inputCurrent == null && current == null)
                return true;

            while (inputCurrent != null && current != null)
            {
                if (!inputCurrent.Equals(current))
                {
                    return false;
                }

                inputCurrent = inputCurrent.Next;
                current = current.Next;
            }

            return true;
        }
    }
}
