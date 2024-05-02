using Data;

namespace UI.Card
{
    /// <summary>
    /// 卡牌数据。
    /// </summary>
    public interface ICardData
    {
        public Data.Card Card => CardData.card;
        
        public CardData CardData { get;}
    }
}