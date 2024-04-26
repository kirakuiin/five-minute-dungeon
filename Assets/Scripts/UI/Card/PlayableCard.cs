using Data.Instruction;
using DG.Tweening;
using GameLib.UI;
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

        private HandZoneSelectorController _selector;

        private Data.Card _card;

        private IPlayerController Controller => GamePlayContext.Instance.GetPlayerController();

        private GamePlayService Service => GamePlayService.Instance;

        /// <summary>
        /// 初始化。
        /// </summary>
        public void Init(Data.Card card, HandZoneUIController zone, HandZoneSelectorController selector)
        {
            _handZone = zone;
            _selector = selector;
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
            if (Service.CanIPlayThisCard(_card))
            {
                RemoveCard();
                Service.PlayCard(_card);
            }
            else
            {
                ShakeCard();
            }
        }

        private void ShakeCard()
        {
            transform.DOShakePosition(0.2f,
                strength: 10f, vibrato: 20, randomness:0, randomnessMode: ShakeRandomnessMode.Harmonic).
                OnComplete(ReturnToOriginPos);
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

        public void SelectCard(bool isSelected)
        {
            if (isSelected)
            {
                _selector.SelectCard(_card);
            }
            else
            {
                _selector.UnSelect(_card);
            }
        }

        public void EnterSelectMode()
        {
            GetComponent<DraggableUI>().enabled = false;
            GetComponent<HandCardSelector>().EnableIt();
        }

        public void ExitSelectMode()
        {
            GetComponent<DraggableUI>().enabled = true;
            GetComponent<HandCardSelector>().DisableIt();
        }
    }
}