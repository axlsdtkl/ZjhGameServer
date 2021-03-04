using MyServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Logic
{
    public interface IHandler
    {
        //断开连接
        void Disconnect(ClientPeer client);
        //接收数据
        //client客户端连接对象
        //subCode子操作码
        //value参数
        void Receive(ClientPeer client, int subCode, object value);
    }
}
