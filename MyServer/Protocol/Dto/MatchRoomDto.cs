using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto
{
    //匹配房间传输模型
    [Serializable]
    public class MatchRoomDto
    {
        //用户ID与该用户UserDto之间的映射字典
        public Dictionary<int, UserDto> userIdUserDtoDic { get; private set; }
        //准备的玩家ID列表
        public List<int> readyUserIdList { get; set; }

        //进入房间顺序的用户ID列表
        public List<int> enterOrderUserIdList { get; private set; }
        //左边玩家ID
        public int LeftPlayerId { get; private set; }
        //右边玩家ID
        public int RightPlayerId { get; private set; }
        public MatchRoomDto()
        {
            userIdUserDtoDic = new Dictionary<int, UserDto>();
            readyUserIdList = new List<int>();
            enterOrderUserIdList = new List<int>();
        }
        //进入房间
        public void Enter(UserDto dto)
        {
            userIdUserDtoDic.Add(dto.UserId, dto);
            enterOrderUserIdList.Add(dto.UserId);
        }
        //离开
        public void Leave(int userId)
        {
            userIdUserDtoDic.Remove(userId);
            readyUserIdList.Remove(userId);
            enterOrderUserIdList.Remove(userId);
        }
        //准备
        public void Ready(int userId)
        {
            readyUserIdList.Add(userId);
        }
        //取消准备
        public void UnReady(int userId)
        {
            readyUserIdList.Remove(userId);
        }
        //重置位置，给三个玩家排序
        public void ResetPosition(int myUserId)
        {
            RightPlayerId = -1;
            LeftPlayerId = -1;
            if (enterOrderUserIdList.Count == 1) return;
            if(enterOrderUserIdList.Count==2)
            {
                //x a
                if(enterOrderUserIdList[0]==myUserId)
                {
                    RightPlayerId = enterOrderUserIdList[1];
                }
                //a x
                if (enterOrderUserIdList[1] == myUserId)
                {
                    LeftPlayerId = enterOrderUserIdList[0];
                }
            }
            if(enterOrderUserIdList.Count==3)
            {
                //x a b
                if(enterOrderUserIdList[0]==myUserId)
                {
                    RightPlayerId = enterOrderUserIdList[1];
                    LeftPlayerId= enterOrderUserIdList[2];
                }
                //a x b
                if (enterOrderUserIdList[1] == myUserId)
                {
                    RightPlayerId = enterOrderUserIdList[2];
                    LeftPlayerId = enterOrderUserIdList[0];
                }
                //a b xx
                if (enterOrderUserIdList[2] == myUserId)
                {
                    RightPlayerId = enterOrderUserIdList[0];
                    LeftPlayerId = enterOrderUserIdList[1];
                }
            }
        }
    }
}
