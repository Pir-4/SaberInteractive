using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;
using SaberInteractive;

namespace UnitTestListRand
{
    [TestFixture()]
    public class ListNodeTests : TestBase
    {
        [SetUp]
        public void Init()
        {
            _listRand = new ListRand();
        }

        [Test]
        public void ListNodeTest01Equals()
        {
            InitDatasAndListRand(2);
            var listNodes = _listRand.ToListNode();

            Assert.IsFalse(listNodes[0].Equals(null), "ListNode equals 'null'");
            Assert.IsFalse(listNodes[0].Equals(string.Empty), "ListNode equals 'string empty'");

            Assert.IsTrue(listNodes[0].Equals(listNodes[0]), "ListNode not equals itself");
            Assert.IsFalse(listNodes[0].Equals(listNodes[1]), "ListNode equals different element");
        }

        [Test]
        public void ListNodeTest02GetHashCode()
        {
            var listNode = new ListNode("test");
            var hashString = listNode.GetHashCode();
            Assert.IsFalse(hashString == default(int), "Hash code ListNode equals 0");

            listNode.Perv = new ListNode("test");
            var hashPerv = CheckPreviousHashCode(listNode.GetHashCode(), hashString);

            listNode.Next = new ListNode("test");
            var hashNext = CheckPreviousHashCode(listNode.GetHashCode(), hashString, hashPerv);

            listNode.Rand = new ListNode("test");
            CheckPreviousHashCode(listNode.GetHashCode(), hashString, hashPerv, hashNext);

            var sameListNode = new ListNode("test");
            Assert.IsTrue(sameListNode.GetHashCode() == hashString, "Hash codes of identical elements are equal");


            InitDatasAndListRand(3);
            var listNodes = _listRand.ToListNode();

            for (int i = 0; i < listNodes.Count; i++)
            {
                for (int j = i + 1; j < listNodes.Count; j++)
                {
                    Assert.IsTrue(listNodes[i].GetHashCode() != listNodes[j].GetHashCode(),
                        $"Different elements have the same hash code." +
                         "Actual #{i}:'{listNodes[i].GetHashCode()}'," +
                         "expected #{j}:'{listNodes[j].GetHashCode()}'");
                }
            }
        }

        private int CheckPreviousHashCode(int currentHash, params int[] previousValues)
        {
            foreach (var pv in previousValues)
            {
                Assert.IsFalse(currentHash == pv, $"Current hash code ListNode equals previous value. " +
                                                  $"Actual {currentHash}, expected {pv}");
            }
            return currentHash;
        }

    }
}
