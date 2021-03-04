﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyServer
{
    public class ClientPeer
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public Socket clientSocket{get;set;}
        private NetMsg msg;
        public ClientPeer()
        {
            msg = new NetMsg();
            ReceiveArgs = new SocketAsyncEventArgs();
            ReceiveArgs.UserToken = this;
            ReceiveArgs.SetBuffer(new byte[2048], 0, 2048);
        }
        #region 接收数据
        //接收的异步套接字操作
        public SocketAsyncEventArgs ReceiveArgs { get; set; }
        //接收到消息之后，存放到数据缓存区
        private List<byte> cache = new List<byte>();
        //是否正在处理接收的数据
        private bool isProcessingReceive = false;
        //消息处理完成后的委托
        public delegate void ReceiveCompleted(ClientPeer client, NetMsg msg);
        public ReceiveCompleted receiveCompleted;
        //处理接收的数据
        public void ProcesReceive(byte[] packet)
        {
            cache.AddRange(packet);
            if (isProcessingReceive == false)
                ProcessData();
        }
        //处理数据
        private void ProcessData()
        {
            isProcessingReceive = true;
            //解析包，从缓存区取出一个完整的包
            byte[] packet = EncodeTool.DecodePacket(ref cache);
            if (packet == null)
            {
                isProcessingReceive = false;
                return;
            }
            NetMsg msg = EncodeTool.DecodeMsg(packet);
            if (receiveCompleted != null)
            {
                receiveCompleted(this, msg);
            }
            ProcessData();

        }
        #endregion
        #region 发送消息
        //发送消息
        public void SendMsg(int opCode, int subCode, object value)
        {
            msg.Change(opCode, subCode, value);
            byte[] data = EncodeTool.EncodeMsg(msg);
            byte[] packet = EncodeTool.EncodePacket(data);
            SendMsg(packet);

        }
        private void SendMsg(byte[] packet)
        {
            try
            {
                clientSocket.Send(packet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        #endregion

        #region 断开连接
        //断开连接
        public void Disconnect()
        {
            cache.Clear();
            isProcessingReceive = false;
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
            clientSocket = null;
        }
        #endregion


    }
}
