using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Cache.Fight
{
    //回合管理类
    public class RoundModel
    {
        //当前下注的玩家
        public int CurrentStakesUserId { get; set; }
        public RoundModel()
        {
            CurrentStakesUserId = -1;
        }
        public void Init()
        {
            CurrentStakesUserId = -1;
        }
        //开始下注
        public void Start(int userId)
        {
            CurrentStakesUserId = userId;
        }
        //轮换下注
        public void Trun(int userId)
        {
            CurrentStakesUserId = userId;
        }
    }
}
