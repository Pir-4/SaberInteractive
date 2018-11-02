﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            var newNode = new ListNode(data) { Rand = randNode };
            Count++;

            if (Head == null)
            {
                Head = newNode;
            }
            else if (Head == Tail)
            {
                Head.Next = newNode;
                newNode.Perv = Head;
            }
            else
            {
                newNode.Perv = Tail;
                Tail.Next = newNode;
            }
            Tail = newNode;
            return newNode;
        }

        public ListNode GetNode(int index, bool isReverse = false)
        {
            if (index < 0 || index >= Count)
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
        }
    }
}
