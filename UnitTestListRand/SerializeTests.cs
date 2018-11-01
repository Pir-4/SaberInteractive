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
        private const string TestFolderPath = @"E:\education\programs\SaberInteractive\Tests";
        private readonly string _serializeFile = Path.Combine(TestFolderPath,"serialize.txt");
        private readonly string _deserializeFile = Path.Combine(TestFolderPath, "deserialize.txt");


        [Test]
        public void Serialize()
        {
            InitDatasAndListRand(3);

            if(File.Exists(_serializeFile))
                File.Delete(_serializeFile);

            using (var fileStream = new FileStream(_serializeFile,FileMode.OpenOrCreate,FileAccess.Write))
            {
                Serializer.Serialize(fileStream, _listRand.Head);
            }
        }
    }
}
