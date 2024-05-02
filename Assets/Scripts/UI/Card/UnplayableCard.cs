using System.Collections.Generic;
using Data;
using GameLib.Common.Extension;
using UI.Common;
using UnityEngine;

namespace UI.Card
{
    /// <summary>
    /// 不可打出的卡牌。
    /// </summary>
    public class UnplayableCard : MonoBehaviour, ICardData
    {
        [SerializeField] private List<InitComponent> initComp;
        
        /// <summary>
        /// 初始化。
        /// </summary>
        public void Init(Data.Card card)
        {
            CardData = DataService.Instance.GetPlayerCardData(card);
            initComp.Apply(obj => obj.Init());
        }

        public CardData CardData { get; private set; }
    }
}