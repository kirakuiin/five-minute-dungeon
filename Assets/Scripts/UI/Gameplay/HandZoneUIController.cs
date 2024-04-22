using System.Collections.Generic;
using System.Linq;
using Data;
using Data.Check;
using GameLib.Common;
using GameLib.Common.DataStructure;
using GameLib.UI.SectorLayout;
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

        [SerializeField] private Transform cardOrigin;

        [SerializeField] private HandZoneSelectorController selector;

        public IReadOnlyList<GameObject> HandCardObjList => _cardList;

        private readonly List<GameObject> _cardList = new ();

        private IPlayerRuntimeInfo _info;

        public void Init(IPlayerRuntimeInfo info)
        {
            selector.Init(this);
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
            var curCard = new Counter<Data.Card>(_cardList.Select(
                cardObj => cardObj.GetComponent<CardRuntimeData>().Card));
            var newCard = new Counter<Data.Card>(_info.GetHands());
            newCard.Subtract(curCard);
            foreach (var card in newCard.Keys)
            {
                var value = newCard[card];
                for (var i = 0; i < value; ++i)
                {
                    layout.Add(CreateCardObj(card));
                }

                if (value < 0)
                {
                    RemoveCardsByType(card, -value);
                }
            }
        }

        private void RemoveCardsByType(Data.Card card, long num)
        {
            var cardObjList = _cardList.Where(
                obj => obj.GetComponent<CardRuntimeData>().Card == card).Select(
                obj => obj).ToList();

            for (var i = 0; i < num; ++i)
            {
                RemoveCard(cardObjList[i]);
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
            layout.Rebuild();
        }
        
        private GameObject CreateCardObj(Data.Card card)
        {
            var cardObj = GameObjectPool.Instance.Get(cardPrefab);
            cardObj.transform.position = cardOrigin.position;
            var setter = cardObj.GetComponent<PlayableCard>();
            setter.Init(card, this, selector);
            _cardList.Add(cardObj);
            return cardObj;
        }

        public void RemoveCard(GameObject cardObj)
        {
            if (!_cardList.Contains(cardObj)) return;
            layout.Remove(cardObj);
            _cardList.Remove(cardObj);
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