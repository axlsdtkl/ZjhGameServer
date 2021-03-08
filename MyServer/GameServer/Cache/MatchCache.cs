using MyServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer.Database;

namespace GameServer.Cache
{
    //匹配缓存层
    public class MatchCache
    {
        //正在匹配的用户ID与房间ID的映射字典
        public Dictionary<int, int> userIdRoomIdDic = new Dictionary<int, int>();
        //正在匹配的房间ID与之对应的房间数据模型映射字典
        public Dictionary<int, MatchRoom> roomIdModelDic = new Dictionary<int, MatchRoom>();

        //重用房间队列
        public Queue<MatchRoom> roomQueue = new Queue<MatchRoom>();
        //线程安全的房间ID
        private ThreadSafeInt roomId = new ThreadSafeInt(-1);
        //进入匹配房间
        public MatchRoom Enter(ClientPeer client)
        {
            //先遍历正在匹配的房间数据模型字典中有没有未满的房间，如果有加进去
            foreach(var mr in roomIdModelDic.Values)
            {
                if (mr.IsFull()) continue;
                mr.Enter(client);
                userIdRoomIdDic.Add(client.Id, mr.roomId);
                return mr;
            }
            //如果执行到这里，代码正在匹配的房间数据模型字典中没有空位了，自己开一个房间
            MatchRoom room = null;
            if (roomQueue.Count > 0)
                room = roomQueue.Dequeue();
            else
                room = new MatchRoom(roomId.Add_Get());
            roomIdModelDic.Add(room.roomId, room);
            userIdRoomIdDic.Add(client.Id, room.roomId);
            return room;
        }
        //离开匹配房间
        public MatchRoom Leave(int userId)
        {
            int roomId = userIdRoomIdDic[userId];
            MatchRoom room = roomIdModelDic[roomId];
            room.Leave(DatabaseManager.GetClientPeerByUserId(userId));
            userIdRoomIdDic.Remove(userId);
            //如果房间为空,将房间加入到房间重用队列，从正在匹配的房间字典中移除掉
            if(room.IsEmpty())
            {
                roomIdModelDic.Remove(roomId);
                roomQueue.Enqueue(room);
            }
            return room;
        }
        //是否在匹配房间里
        public bool IsMatching(int userId)
        {
            return userIdRoomIdDic.ContainsKey(userId);
        }
        //获取玩家所在的房间
        public MatchRoom GetRoom(int userId)
        {
            int roomId = userIdRoomIdDic[userId];
            return roomIdModelDic[roomId];

        }
        //销毁房间 游戏开始时调用
        public void DestoryRoom(MatchRoom room)
        {
            roomIdModelDic.Remove(room.roomId);
            foreach(var client in room.clientList)
            {
                userIdRoomIdDic.Remove(client.Id);
            }
            room.clientList.Clear();
            room.readyUIdList.Clear();
            roomQueue.Enqueue(room);
        }
    }

}
