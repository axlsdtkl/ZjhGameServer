﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto
{
    //用户信息传输模型
    [Serializable]
    public class UserDto
    {
        public int UserId;
        public string UserName;
        public string IconName;
        public int CoinCount;

        public UserDto(int UserId, string UserName, string IconName, int CoinCount)
        {
            this.UserId = UserId;
            this.UserName = UserName;
            this.IconName = IconName;
            this.CoinCount = CoinCount;
        }
        public void Change(int UserId, string UserName, string IconName, int CoinCount)
        {
            this.UserId = UserId;
            this.UserName = UserName;
            this.IconName = IconName;
            this.CoinCount = CoinCount;
        }
    }
}
