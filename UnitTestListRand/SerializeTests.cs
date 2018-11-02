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
        private readonly string _serializeFile = Path.Combine(TestFolderPath,"serialize.txt");
        private readonly string _deserializeFile = Path.Combine(TestFolderPath, "deserialize.txt");

        [TestCase(5)]
        [TestCase(0)]
        [TestCase(100)]
        public void SerializeCountTest(int count)
        {
            var datas = InitDatasAndListRand(count);

            if(File.Exists(_serializeFile))
                File.Delete(_serializeFile);

            using (var fileStream = new FileStream(_serializeFile,FileMode.OpenOrCreate,FileAccess.Write))
            {
                Serializer.Serialize(fileStream, _listRand.Head);
            }

            using (var reader = new StreamReader(_serializeFile))
            {
                foreach (var data in datas)
                {
                    var actualValue = reader.ReadLine();
                    Assert.IsTrue(actualValue.Contains(data), 
                        $"Serialize string not contains expected data." +
                        $"Actual '{actualValue}', expected {data}");
                }
            }
        }
    }
}
