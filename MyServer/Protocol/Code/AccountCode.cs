﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Code
{
    //账户的子操作码
    public class AccountCode
    {
        public const int Register_CREQ= 0;
        public const int Register_SRES = 1;
        public const int Login_CREQ = 2;
        public const int Login_SRES = 3;
        public const int GetUserInfo_CREQ = 4;
        public const int GetUserInfo_SRES = 5;
        public const int GetRankList_CREQ = 6;
        public const int GetRankList_SRES = 7;

        public const int UpdateCoinCount_CREQ = 8;
        public const int UpdateCoinCount_SRES = 9;

        public const int Logout_CREQ = 10;
        public const int Logout_SRES = 11;

        public const int ModifyPwd_CREQ = 12;
        public const int ModifyPwd_SRES = 13;

    }
}
