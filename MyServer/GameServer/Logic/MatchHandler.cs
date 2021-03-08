using GameServer.Cache;
using MyServer;
using Protocol.Code;
using Protocol.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer.Database;

namespace GameServer.Logic
{
    public class MatchHandler : IHandler
        //匹配房间缓存集合
    {
        private List<MatchCache> matchCacheList = Caches.matchCacheList;
        public void Disconnect(ClientPeer client)
        {
        }

        public void Receive(ClientPeer client, int subCode, object value)
        {
            switch(subCode)
            {
                case MatchCode.Enter_CREQ:
                    EnterRoom(client, (int)value);
                    break;
                default:
                    break;

            }
        }
        //客户端进入房间的请求
        private void EnterRoom(ClientPeer client,int roomType)
        {
            SingleExecute.Instance.Execute(() =>
            {
                //判断一下当前客户端连接对象是不是在匹配房间里面，如果在，则忽略
                if (matchCacheList[roomType].IsMatching(client.Id)) return;
                MatchRoom room=matchCacheList[roomType].Enter(client);
                //构造UserDto用户数据传输模型
                UserDto userDto = DatabaseManager.CreatUserDto(client.Id);
                //广播给房间内的所有玩家，除了自身，有新的玩家进来了，参数:新进用户的UserDto
                room.Broadcast(OpCode.Match, MatchCode.Enter_BRO, userDto, client);

                //给客户端一个响应  参数：房间传输模型，包含房间内的正在等待的玩家以及准备的玩家id集合
                //TODO
                if(roomType==0)
                {
                    Console.WriteLine(userDto.UserName + "进入底注为10，项注为100的房间");
                }
                if (roomType == 1)
                {
                    Console.WriteLine(userDto.UserName + "进入底注为20，项注为200的房间");
                }
                if (roomType == 2)
                {
                    Console.WriteLine(userDto.UserName + "进入底注为50，项注为500的房间");
                }
            });
        }
    }
}
