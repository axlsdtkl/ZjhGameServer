using MyServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Cache
{
    //匹配房间
    public class MatchRoom
    {
        //房间Id,唯一标记
        public int roomId { get; private set; }
        //房间内的玩家
        public List<ClientPeer> clientList { get; private set; }
        //房间内准备的玩家ID列表
        public List<int> readyUIdList { get; set; } 

        public MatchRoom(int Id)
        {
            roomId = Id;
            clientList = new List<ClientPeer>();
            readyUIdList = new List<int>();
        }
        //获取房间是否满了
        public bool IsFull()
        {
            return clientList.Count == 3;
        }
        //获取房间是否为空
        public bool IsEmpty()
        {
            return clientList.Count == 0;
        }
        //获取是否全部玩家准备，如果返回值为true，就可以开始游戏了
        public bool IsAllReady()
        {
            return readyUIdList.Count == 3;
        }

        //进入房间
        public void Enter(ClientPeer client)
        {
            clientList.Add(client);
        }
        //离开房间
        public void Leave(ClientPeer client)
        {
            clientList.Remove(client);
            if(readyUIdList.Contains(client.Id))
            {
                readyUIdList.Remove(client.Id);
            }
        }
        //准备
        public void Ready(int userId)
        {
            readyUIdList.Add(userId);
        }
        //取消准备
        public void UnReady(int userId)
        {
            readyUIdList.Remove(userId);
        }
        //广播发消息
        public void Broadcast(int opCode,int subCode,object value,ClientPeer exceptClient=null)
        {
            NetMsg msg = new NetMsg(opCode, subCode, value);
            byte[] data = EncodeTool.EncodeMsg(msg);
            byte[] packet=EncodeTool.EncodePacket(data);
            foreach(var client in clientList)
            {
                if (client == exceptClient) continue;
                client.SendMsg(packet);
            }
        }
    }
}
