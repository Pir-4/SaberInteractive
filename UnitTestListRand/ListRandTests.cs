using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SaberInteractive;

namespace UnitTestListRand
{
    [TestFixture]
    public class ListRandTests : TestBase
    {

        [SetUp]
        public void Init()
        {
            _listRand = new ListRand();
        }

        [TestCase(5)]
        [TestCase(0)]
        [TestCase(100)]
        public void ListRandTest01AddItemsAndCheckCount(int count)
        {
            Assert.IsTrue(_listRand.Count.Equals(0),
                $"ListRand isn't empty");

            var datas = InitDatasAndListRand(count);

            Assert.IsTrue(_listRand.Count.Equals(datas.Count()),
                $"Counts not equals. ListRand {_listRand.Count}, expected {datas.Count()}");
        }

        [TestCase(5, false)]
        [TestCase(0,false)]
        [TestCase(100, false)]
        [TestCase(5, true)]
        [TestCase(0, true)]
        [TestCase(100, true)]
        public void ListRandTest02DataEqualsItems(int count, bool isReverse)
        {
            Assert.IsFalse(_listRand.ToListString().Any(),
                $"ListRand isn't empty");

            var datas = InitDatasAndListRand(count);

            if (isReverse)
            {
                datas.Reverse();
            }

            var listString = _listRand.ToListString(isReverse);
            
            Assert.IsTrue(listString.Count.Equals(datas.Count),
                $"Counts not equals. ListRand {listString.Count}, expected count {datas.Count}");

            for (int i = 0; i < datas.Count; i++)
            {
                Assert.IsTrue(listString[i].Equals(datas[i]),
                    $"Items not equals. Actual '{listString[i]}', expected {datas[i]}");
            }
        }

        [TestCase(5, false)]
        [TestCase(0, false)]
        [TestCase(100, false)]
        [TestCase(5, true)]
        [TestCase(0, true)]
        [TestCase(100, true)]
        public void ListRandTest03GetNodeItems(int count, bool isReverse)
        {
            Assert.IsFalse(_listRand.ToListString().Any(),
                $"ListRand isn't empty");

            var datas = InitDatasAndListRand(count);

            if (isReverse)
            {
                datas.Reverse();
            }

            Assert.IsTrue(_listRand.Count.Equals(datas.Count),
                $"Counts not equals. ListRand {_listRand.Count}, expected count {datas.Count}");

            for (int i = 0; i < datas.Count; i++)
            {
                var node = _listRand.GetNode(i, isReverse);
                Assert.IsTrue(node.Data.Equals(datas[i]),
                    $"Items not equals. Actual '{node.Data}', expected {datas[i]}");
            }
        }
    }
}