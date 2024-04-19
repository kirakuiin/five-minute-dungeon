using Data.Instruction;
using Gameplay.Core;
using UI.Gameplay;
using UnityEngine;

namespace UI.Card
{
    /// <summary>
    /// 代表可以打出的手牌。
    /// </summary>
    public class PlayableCard : MonoBehaviour
    {
        private HandZoneUIController _handZone;

        private Data.Card _card;

        private IPlayerController Controller => GamePlayContext.Instance.GetPlayerController();

        /// <summary>
        /// 初始化。
        /// </summary>
        public void Init(Data.Card card, HandZoneUIController zone)
        {
            _handZone = zone;
            _card = card;
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
            GamePlayService.Instance.PlayCard(_card);
        }

        /// <summary>
        /// 丢弃卡牌。
        /// </summary>
        public void DiscardCard()
        {
            RemoveCard();
            Controller.Discard(new []{_card});
        }
        
        private void RemoveCard()
        {
            _handZone.RemoveCard(gameObject);
        }
    }
}