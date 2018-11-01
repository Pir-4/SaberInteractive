using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace UnitTestListRand
{
    [TestFixture()]
    public class ListNodeTests : ListRandTests
    {
        [Test]
        public void CheckMethodEqualsListNodeTest()
        {
            InitDatasAndListRand(2);
            var listNode = _listRand.ToListNodes();

            Assert.IsFalse(listNode[0].Equals(null), "ListNode equals 'null'");
            Assert.IsFalse(listNode[0].Equals(string.Empty), "ListNode equals 'string empty'");

            Assert.IsTrue(listNode[0].Equals(listNode[0]), "ListNode not equals itself");
            Assert.IsFalse(listNode[0].Equals(listNode[1]), "ListNode not equals itself");
        }
    }
}
