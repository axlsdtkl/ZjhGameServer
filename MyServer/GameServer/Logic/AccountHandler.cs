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
    public class AccountHandler : IHandler
    {
        public void Disconnect(ClientPeer client)
        {
            DatabaseManager.OffLine(client);

        }
        public void Receive(ClientPeer client, int subCode, object value)
        {
            switch(subCode)
            {
                case AccountCode.Register_CREQ:
                    Register(client,value as AccountDto);
                    break;
                case AccountCode.Login_CREQ:
                    Login(client, value as AccountDto);
                    break;
                case AccountCode.GetUserInfo_CREQ:
                    GetUserInfo(client);
                    break;
                case AccountCode.GetRankList_CREQ:
                    GetRankList(client);
                    break;
                case AccountCode.UpdateCoinCount_CREQ:
                    UpdateCoinCount(client,(int)value);
                    break;
                case AccountCode.ModifyPwd_CREQ:
                    ModifyPwd(client, value as AccountDto);
                    break;
                case AccountCode.Logout_CREQ:
                    UserLogout(client);
                    break;
                default:
                    break;
            }
        }
        //客户端发现更新Coin请求
        private void UpdateCoinCount(ClientPeer client,int coinCount)
        {
            SingleExecute.Instance.Execute(() =>
            {
                int totalCoin=DatabaseManager.UpdateCoinCount(client.Id, coinCount);
                client.SendMsg(OpCode.Account, AccountCode.UpdateCoinCount_SRES, totalCoin);
            });
        }
        //客户端更新账号信息
        private void ModifyPwd(ClientPeer client, AccountDto dto)
        {
            SingleExecute.Instance.Execute(() =>
            {
                int flag = DatabaseManager.ModifyPwd(client, dto.password);
                client.SendMsg(OpCode.Account, AccountCode.ModifyPwd_SRES, flag);
            });
        }

        //客户端获取排行榜的请求处理
        private void GetRankList(ClientPeer client)
        {
            SingleExecute.Instance.Execute(() =>
            {
                RankListDto dto = DatabaseManager.GetRankListDto();
                client.SendMsg(OpCode.Account, AccountCode.GetRankList_SRES, dto);
            });
        }
        //客户端获取用户信息的请求
        private void GetUserInfo(ClientPeer client)
        {
            SingleExecute.Instance.Execute(()=>
                {
                    UserDto dto=DatabaseManager.CreatUserDto(client.Id);
                    client.SendMsg(OpCode.Account, AccountCode.GetUserInfo_SRES,dto);

            });
        }
        //用户登出
        private void UserLogout(ClientPeer client)
        {
            DatabaseManager.OffLine(client);
            client.SendMsg(OpCode.Account, AccountCode.Logout_SRES, 0);
            return;
        }
        //客户端登录的请求
        private void Login(ClientPeer client,AccountDto dto)
        {
            
            SingleExecute.Instance.Execute(() =>
            {
                if(DatabaseManager.IsExistUserName(dto.userName)==false)
                {
                    //用户名不存在
                    client.SendMsg(OpCode.Account, AccountCode.Login_SRES, -1);
                    return;
                }
                if(DatabaseManager.IsMatch(dto.userName,dto.password)==false)
                {
                    //密码不正确
                    client.SendMsg(OpCode.Account, AccountCode.Login_SRES, -2);
                    return;
                }
                if (DatabaseManager.isOnline(dto.userName))
                {
                    //该账号已经在线
                    client.SendMsg(OpCode.Account, AccountCode.Login_SRES, -3);
                    return;
                }
                DatabaseManager.Login(dto.userName, client);
                //登录成功
                client.SendMsg(OpCode.Account, AccountCode.Login_SRES, 0);
            });
        }
        //客户端注册的处理
        private void Register(ClientPeer  client,AccountDto dto)
        {
            //单线程执行
            //防止多个线程同时访问数据出错
            SingleExecute.Instance.Execute(() =>
            {
                //用户名已被注册
                if (DatabaseManager.IsExistUserName(dto.userName))
                {
                    client.SendMsg(OpCode.Account, AccountCode.Register_SRES, -1);
                    return;
                }
                //创建一条用户数据
                DatabaseManager.CreatUser(dto.userName, dto.password);
                client.SendMsg(OpCode.Account, AccountCode.Register_SRES, 0);
            });
            
        }
    }
}
