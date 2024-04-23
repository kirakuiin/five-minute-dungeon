using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using Data.Check;
using GameLib.Common;
using GameLib.Common.DataStructure;
using GameLib.UI.SectorLayout;
using Gameplay.Core;
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

        private IPlayerRuntimeInfo RuntimeInfo => GamePlayContext.Instance.GetPlayerRuntimeInfo();

        public void Init()
        {
            selector.Init(this);
            InitListen();
            StartCoroutine(InitUI());
        }

        private void InitListen()
        {
            RuntimeInfo.GetHands().OnCardChanged += OnCardChanged;
        }

        private void OnCardChanged(CardChangeEvent e)
        {
            var curCard = new Counter<Data.Card>(_cardList.Select(
                cardObj => cardObj.GetComponent<CardRuntimeData>().Card));
            var diff = new Counter<Data.Card>(RuntimeInfo.GetHands());
            diff.Subtract(curCard);
            SetSectorIntervalByHandCount(RuntimeInfo.GetHands().Count);
            StartCoroutine(ExecuteCardChange(diff));
        }

        private void SetSectorIntervalByHandCount(long count)
        {
            var x = count;
            var interval = -0.001466666666666703*x*x*x+0.06800000000000087*x*x-1.0633333333333392*x+6.800000000000011;
            layout.SetAngle((float)interval);
        }

        private IEnumerator ExecuteCardChange(Counter<Data.Card> diff)
        {
            foreach (var card in diff.Keys)
            {
                var value = diff[card];
                for (var i = 0; i < value; ++i)
                {
                    layout.Add(CreateCardObj(card));
                    yield return new WaitForEndOfFrame();
                }

                if (value < 0)
                {
                    RemoveCardsByType(card, -value);
                }
            }
            layout.Rebuild();
            yield return new WaitForEndOfFrame();
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

        private IEnumerator InitUI()
        {
            foreach (var card in RuntimeInfo.GetHands())
            {
                layout.Add(CreateCardObj(card));
                yield return new WaitForEndOfFrame();
            }
            layout.Rebuild();
            yield return new WaitForEndOfFrame();
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