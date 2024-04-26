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

        private readonly List<GameObject> _cardList = new ();

        private IPlayerRuntimeInfo RuntimeInfo => GamePlayContext.Instance.GetPlayerRuntimeInfo();

        private bool _isSelectMode;

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
            var interval = -0.00040000000000002447*x*x*x+0.022000000000000797*x*x-0.4600000000000055*x+5.800000000000011;
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
                    yield return null;
                }

                if (value < 0)
                {
                    RemoveCardsByType(card, -value);
                }
            }
            layout.Rebuild();
            ChangeCardSelectMode();
            yield return null;
        }

        private void RemoveCardsByType(Data.Card card, long num)
        {
            var cardObjList = _cardList.Where(
                obj => obj.GetComponent<CardRuntimeData>().Card == card).Select(
                obj => obj).ToList();

            for (var i = 0; i < num; ++i)
            {
                PlayDiscardAnim(cardObjList[i]);
                RemoveCard(cardObjList[i]);
            }
        }

        private IEnumerator InitUI()
        {
            foreach (var card in RuntimeInfo.GetHands())
            {
                layout.Add(CreateCardObj(card));
                yield return null;
            }
            layout.Rebuild();
            yield return null;
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

        private void PlayDiscardAnim(GameObject cardObj)
        {
            var discardEffect = GetComponent<CardDiscardEffect>();
            discardEffect.Discard(cardObj);
        }

        /// <summary>
        /// 重置卡牌位置。
        /// </summary>
        public void ResetAllCardPos()
        {
            layout.Rebuild();
        }

        public void EnterSelectMode()
        {
            _isSelectMode = true;
            ChangeCardSelectMode();
        }

        private void ChangeCardSelectMode()
        {
            foreach (var obj in _cardList)
            {
                if (_isSelectMode)
                {
                    obj.GetComponent<PlayableCard>().EnterSelectMode();
                }
                else
                {
                    obj.GetComponent<PlayableCard>().ExitSelectMode();
                }
            }
        }

        public void ExitSelectMode()
        {
            _isSelectMode = false;
            ChangeCardSelectMode();
        }
    }
}