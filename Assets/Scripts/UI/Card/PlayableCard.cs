using System.Collections.Generic;
using Data;
using Data.Instruction;
using DG.Tweening;
using GameLib.Common.Extension;
using GameLib.UI;
using Gameplay.Core;
using UI.Common;
using UI.Gameplay;
using UnityEngine;

namespace UI.Card
{
    /// <summary>
    /// 代表可以打出的手牌。
    /// </summary>
    public class PlayableCard : MonoBehaviour, ICardData
    {
        [SerializeField] private List<InitComponent> initComp;
        
        private HandZoneUIController _handZone;

        private HandZoneSelectorController _selector;
        
        private IPlayerController Controller => GamePlayContext.Instance.GetPlayerController();

        private GamePlayService Service => GamePlayService.Instance;

        public CardData CardData { get; private set; }
        
        /// <summary>
        /// 初始化。
        /// </summary>
        public void Init(Data.Card card, HandZoneUIController zone, HandZoneSelectorController selector)
        {
            _handZone = zone;
            _selector = selector;
            CardData = DataService.Instance.GetPlayerCardData(card);
            initComp.Apply(obj => obj.Init());
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
            if (Service.Status.CanIPlayThisCard(CardData.card))
            {
                RemoveCard();
                Service.PlayCard(CardData.card);
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
            Controller.Discard(new []{CardData.card});
            Controller.FillHands();
        }
        
        private void RemoveCard()
        {
            _handZone.RemoveCard(gameObject);
        }

        public void SelectCard(bool isSelected)
        {
            if (isSelected)
            {
                _selector.SelectCard(CardData.card);
            }
            else
            {
                _selector.UnSelect(CardData.card);
            }
        }

        public void EnterSelectMode()
        {
            SetDraggable(false);
            GetComponent<HandCardSelector>().EnableIt();
        }

        public void ExitSelectMode()
        {
            SetDraggable(true);
            GetComponent<HandCardSelector>().DisableIt();
        }

        public void SetDraggable(bool isDraggable)
        {
            GetComponent<DraggableUI>().enabled = isDraggable;
        }
    }
}