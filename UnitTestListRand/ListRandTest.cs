using System;
using System.Linq;
using NUnit.Framework;
using SaberInteractive;

namespace UnitTestListRand
{
    [TestFixture]
    public class ListRandTest
    {
        private ListRand _listRand;


        [TestCase(5)]
        [TestCase(0)]
        [TestCase(100)]
        public void AddItemsAndCheckCount(int count)
        {
            _listRand = new ListRand();
            Assert.IsTrue(_listRand.Count.Equals(0),
                $"ListRand isn't empty");

            var datas = Enumerable.Range(0, count).Select(_ => Guid.NewGuid().ToString()).ToList();

            foreach (var data in datas)
            {
                _listRand.Add(data);
            }

            Assert.IsTrue(_listRand.Count.Equals(datas.Count()),
                $"Counts not equals. ListRand {_listRand.Count}, expected {datas.Count()}");
        }

        [TestCase(5, false)]
        [TestCase(0,false)]
        [TestCase(100, false)]
        [TestCase(5, true)]
        [TestCase(0, true)]
        [TestCase(100, true)]
        public void EqualsItems(int count, bool isReverse)
        {
            _listRand = new ListRand();
            Assert.IsFalse(_listRand.ToList().Any(),
                $"ListRand isn't empty");

            var datas = Enumerable.Range(0, count).Select(_ => Guid.NewGuid().ToString()).ToList();

            foreach (var data in datas)
            {
                _listRand.Add(data);
            }
            if (isReverse)
            {
                datas.Reverse();
            }
            var listString = _listRand.ToList(isReverse);
            
            Assert.IsTrue(listString.Count.Equals(datas.Count),
                $"Counts not equals. ListRand {_listRand.Count}, expected count {datas.Count}");

            for (int i = 0; i < datas.Count; i++)
            {
                Assert.IsTrue(listString[i].Equals(datas[i]),
                    $"Items not equals. Actual '{listString[i]}', expected {datas[i]}");
            }
        }
    }
}
