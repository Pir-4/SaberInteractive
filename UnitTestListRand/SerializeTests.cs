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
   public class SerializeTests : ListRandTests
    {

        [Test]
        public void Serialize()
        {
            var datas = InitDatasAndListRand(3);
        }
    }
}
