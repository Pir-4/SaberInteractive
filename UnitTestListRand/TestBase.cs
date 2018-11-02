using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SaberInteractive;

namespace UnitTestListRand
{
    public class TestBase
    {
        protected ListRand _listRand;

        protected List<string> InitDatasAndListRand(int count)
        {
            _listRand = _listRand ?? new ListRand();
            var datas = Enumerable.Range(0, count).Select(_ => Guid.NewGuid().ToString()).ToList();

            foreach (var data in datas)
            {
                _listRand.Add(data);
            }
            return datas;
        }

        protected void InitListNodeRandomItem(bool isReverse = false)
        {
            var rand = new Random(_listRand.GetHashCode());

            foreach (var node in _listRand.ToListNode())
            {
                node.Rand = _listRand.GetNode(rand.Next(0, _listRand.Count), isReverse);
            }
        }
    }
}
