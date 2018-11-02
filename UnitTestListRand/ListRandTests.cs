using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
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

            var datas = InitDatasAndListRandByGuid(count);

            Assert.IsTrue(_listRand.Count.Equals(datas.Count()),
                $"Counts not equals. ListRand {_listRand.Count}, expected {datas.Count()}");
        }

        [TestCase(5, false)]
        [TestCase(0, false)]
        [TestCase(100, false)]
        [TestCase(5, true)]
        [TestCase(0, true)]
        [TestCase(100, true)]
        public void ListRandTest02DataEqualsItems(int count, bool isReverse)
        {
            Assert.IsFalse(_listRand.ToListString().Any(),
                $"ListRand isn't empty");

            var datas = InitDatasAndListRandByGuid(count);

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

            var datas = InitDatasAndListRandByGuid(count);

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

        [Test]
        public void ListRandTest04GetNodeConfines()
        {
            InitDatasAndListRandByGuid(3);
            Assert.IsTrue(_listRand.GetNode(_listRand.Count) == null, $"List return item with numbers equals count {_listRand.Count}");

            try
            {
                _listRand.GetNode(-1);
                Assert.IsTrue(false, "List not return exception");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true);
            }
        }

        [TestCase(5)]
        [TestCase(0)]
        [TestCase(100)]
        public void ListRandTest05Equals(int count)
        {
            InitDatasAndListRandByGuid(count);

            Assert.IsFalse(_listRand.Equals(null), "ListRand equals 'null'");
            Assert.IsFalse(_listRand.Equals(new DateTime()), "ListRand equals 'date time'");

            Assert.IsTrue(_listRand.Equals(_listRand), "ListRand not equals itself");

            if (count > 0)
            {
                var likeCountListRand = new ListRand();
                for (int i = 0; i < _listRand.Count; i++)
                {
                    likeCountListRand.Add(Guid.NewGuid().ToString());
                }

                Assert.IsFalse(likeCountListRand.Equals(_listRand), "ListRands equals same count list");
            }

            Assert.IsTrue(new ListRand().Equals(new ListRand()), "ListRand with heads equals null not equals");


            var likeListRand = new ListRand();
            foreach (var node in _listRand.ToListNode())
            {
                likeListRand.Add(node);
            }

            Assert.IsTrue(likeListRand.Equals(_listRand), "ListRand not equals same list");

        }

        [TestCase(5)]
        [TestCase(0)]
        [TestCase(100)]
        public void ListRandTest06SerializeDeserialize(int count)
        {
            Serialize(_serializeFile, count);

            var deserializeList = new ListRand();

            using (var fileStream = new FileStream(_serializeFile, FileMode.Open, FileAccess.Read))
            {
                deserializeList.Deserialize(fileStream);
            }

            Assert.IsTrue(_listRand.Equals(deserializeList), "ListRands not equals");

        }

        protected void Serialize(string serializeFile, int count)
        {
            InitDatasAndListRandByGuid(count);
            InitListNodeRandomItem();

            if (File.Exists(_serializeFile))
                File.Delete(_serializeFile);

            using (var fileStream = new FileStream(serializeFile, FileMode.OpenOrCreate, FileAccess.Write))
            {
               _listRand.Serialize(fileStream);
            }
        }
    }
}