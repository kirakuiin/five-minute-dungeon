using System;
using GameLib.Common;
using GameLib.UI.SectorLayout;
using Gameplay.Core;
using Gameplay.Data;
using UI.Card;
using UnityEngine;

namespace UI.Gameplay
{
    /// <summary>
    /// 手牌UI控制。
    /// </summary>
    public class HandZoneUIController : MonoBehaviour
    {
        [SerializeField] private SectorLayout layout;

        [SerializeField] private GameObject cardPrefab;

        private IPlayerRuntimeInfo _info;

        public void Init(IPlayerRuntimeInfo info)
        {
            _info = info;
            InitListen();
            InitUI();
        }

        private void InitListen()
        {
            _info.GetHands().OnCardChanged += OnCardChanged;
        }

        private void OnCardChanged(CardChangeEvent e)
        {
            if (e.type == CardChangeType.RemoveCard) return;
            foreach (var card in e.cardList)
            {
                layout.Add(CreateCardObj(card));
            }
        }

        private void OnDestroy()
        {
            _info.GetHands().OnCardChanged -= OnCardChanged;
        }

        private void InitUI()
        {
            foreach (var card in _info.GetHands())
            {
                layout.Add(CreateCardObj(card));
            }
        }
        
        private GameObject CreateCardObj(Data.Card card)
        {
            var cardObj = GameObjectPool.Instance.Get(cardPrefab);
            var setter = cardObj.GetComponent<PlayableCard>();
            setter.Init(card, this);
            return cardObj;
        }

        /// <summary>
        /// 移除卡牌。
        /// </summary>
        /// <param name="cardObj"></param>
        public void RemoveCard(GameObject cardObj)
        {
            layout.Remove(cardObj);
            GameObjectPool.Instance.ReturnWithReParent(cardObj, cardPrefab); 
        }

        /// <summary>
        /// 重置卡牌位置。
        /// </summary>
        public void ResetAllCardPos()
        {
            layout.Rebuild();
        }
    }
}