using Data.Instruction;
using Gameplay.Core;
using UI.Gameplay;
using Unity.Netcode;
using UnityEngine;

namespace UI.Card
{
    /// <summary>
    /// 代表可以打出的手牌。
    /// </summary>
    public class PlayableCard : MonoBehaviour
    {
        private HandZoneUIController _handZone;

        
        /// <summary>
        /// 初始化。
        /// </summary>
        public void Init(Data.Card card, HandZoneUIController zone)
        {
            _handZone = zone;
            GetComponent<CardRuntimeData>().Init(card);
            GetComponent<CardAppearanceSetter>().Init();
        }

        public void ReturnToOriginPos()
        {
            _handZone.ResetAllCardPos();
        }
        
        /// <summary>
        /// 打出卡牌。
        /// </summary>
        public void PlayCard()
        {
            RemoveCard();
            GamePlayService.Instance.PlayCard(GetComponent<CardRuntimeData>().Card);
        }

        /// <summary>
        /// 丢弃卡牌。
        /// </summary>
        public void DiscardCard()
        {
            RemoveCard();
            GamePlayService.Instance.DiscardCard(GetComponent<CardRuntimeData>().Card);
        }
        
        private void RemoveCard()
        {
            _handZone.RemoveCard(gameObject);
        }
    }
}