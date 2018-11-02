using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;
using SaberInteractive;

namespace UnitTestListRand
{
    [TestFixture()]
    public class SerializeTests : ListRandTests
    {
        private const string TestFolderPath = @"E:\Git\My\SaberInteractive\TestFiles";
        private readonly string _serializeFile = Path.Combine(TestFolderPath, "serialize.txt");
        private readonly string _deserializeFile = Path.Combine(TestFolderPath, "deserialize.txt");

        [TestCase(5)]
        [TestCase(0)]
        [TestCase(100)]
        public void SerializeTest01SerializeCheckContainsDataInLine(int count)
        {
            Serialize(_serializeFile, count);

            using (var reader = new StreamReader(_serializeFile))
            {
                if (_listRand.IsEmpty)
                {
                    Assert.IsTrue(reader.EndOfStream, "File not empty");
                }
                else
                {
                    foreach (var node in _listRand.ToListNode())
                    {
                        var actualValue = reader.ReadLine();
                        Assert.IsTrue(actualValue.Contains(node.Data),
                            $"Serialize string 'Data' not contains expected data." +
                            $"Actual '{actualValue}', expected {node.Data}");

                        ActualValueContains(actualValue, node.Perv, "Perv.Guid");
                        ActualValueContains(actualValue, node.Next, "Next.Guid");
                        ActualValueContains(actualValue, node.Rand, "Rand.Guid");
                    }
                }
            }
        }

        [TestCase(5)]
       // [TestCase(0)]
       // [TestCase(100)]
        public void SerializeTest02(int count)
        {
            Serialize(_serializeFile, count);

            using (var fileStream = new FileStream(_serializeFile,FileMode.Open, FileAccess.Read))
            {
                Serializer.Deserialize(fileStream);
            }
        }

        private void Serialize(string serializeFile, int count)
        {
            InitDatasAndListRand(count);
            InitListNodeRandomItem();

            if (File.Exists(_serializeFile))
                File.Delete(_serializeFile);

            using (var fileStream = new FileStream(serializeFile, FileMode.OpenOrCreate, FileAccess.Write))
            {
                Serializer.Serialize(fileStream, _listRand.Head);
            }
        }

        private static void ActualValueContains(string actualValue, ListNode node, string fieldName)
        {
            if (node != null)
            {
                Assert.IsTrue(actualValue.Contains(node.Guid.ToString()),
                    $"Serialize string '{fieldName}' not contains expected data." +
                    $"Actual '{actualValue}', expected '{node.Guid}'");
            }
        }
    }
}
