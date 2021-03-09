using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto.Fight
{
    //卡牌传输模型
    [Serializable]
    public class CardDto
    {
        public string cardName;
        public int weight;
        public int color;

        public CardDto(string cardName,int weight,int color)
        {
            this.cardName = cardName;
            this.weight = weight;
            this.color = color;
        }
    }
}
