using MyServer;
using Protocol.Constant;
using Protocol.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Cache.Fight
{
    //战斗房间
    public class FightRoom
    {
        //房间ID，唯一标识
        public int roomId;
        //玩家列表
        public List<PlayerDto> playerList;

        //牌库
        public CardLibrary cardLibrary;
        //回合管理类
        public RoundModel roundModel;
        //离开的玩家列表
        public List<int> leaveUserIdList;
        //弃牌的玩家列表
        public List<int> giveUpCardUserIdList;
        //顶注
        public int topStakes;
        //底注
        public int bottomStakes;
        //上一位玩家下注的数量 
        public int lastPlayerStakesCount;
        //总下注数
        public int stakesSum;
        //庄家在玩家列表中的下标
        private int bankerIndex=-1;
        public FightRoom(int roomId,List<ClientPeer> clientList)
        {
            this.roomId = roomId;
            playerList = new List<PlayerDto>();
            foreach(var client in clientList)
            {
                PlayerDto dto = new PlayerDto(client.Id,client.UserName);
                playerList.Add(dto);
            }
            cardLibrary = new CardLibrary();
            roundModel = new RoundModel();
            leaveUserIdList = new List<int>();
            giveUpCardUserIdList = new List<int>();
            stakesSum = 0;
        }
        public void Init(List<ClientPeer> clientList)
        {
            stakesSum = 0;
            playerList.Clear();
            foreach (var client in clientList)
            {
                PlayerDto dto = new PlayerDto(client.Id, client.UserName);
                playerList.Add(dto);
            }
        }
        //选择庄家
        public ClientPeer SetBanker()
        {
            Random ran = new Random();
            int ranIndex=ran.Next(0, playerList.Count);
            bankerIndex = ranIndex;
            //随机到的庄家用户ID
            int userId = playerList[ranIndex].userId;
            playerList[ranIndex].identity = Identity.Banker;
            //设置庄家为默认下注者
            roundModel.Start(userId);
            ClientPeer bankerClient = Database.DatabaseManager.GetClientPeerByUserId(userId);
            string userName = bankerClient.UserName;
            Console.WriteLine("庄家为:" + userName);
            return bankerClient;
        }
        //发牌
        public void DealCards()
        {
            for(int i=0;i<9;i++)
            {
                playerList[bankerIndex].AddCard(cardLibrary.DealCard());
                bankerIndex++;
                if(bankerIndex>playerList.Count-1)
                {
                    bankerIndex = 0;
                }
            }
        }
    }
}
