using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Cache
{
    //数据缓存单一模式
    //防止数据缓存存在多份，打破了数据的唯一性
    public class Caches
    {
        public static List<MatchCache> matchCacheList { get; set; }
        static Caches()
        {
            matchCacheList = new List<MatchCache>();
            for(int i=0;i<3;i++)
            {
                matchCacheList.Add(new MatchCache());
            }
        }
    }
}
