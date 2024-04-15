using Data;
using UnityEngine;

namespace UI.Card
{
    /// <summary>
    /// 卡牌运行时数据。
    /// </summary>
    public class CardRuntimeData : MonoBehaviour
    {
        /// <summary>
        /// 初始化。
        /// </summary>
        /// <param name="card"></param>
        public void Init(Data.Card card)
        {
            CardData = DataService.Instance.GetPlayerCardData(card);
        }

        public Data.Card Card => CardData.card;

        public CardData CardData { get; private set; }
    }
}