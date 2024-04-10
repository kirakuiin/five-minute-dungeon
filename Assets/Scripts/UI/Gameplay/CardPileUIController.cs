using System.Collections.Generic;
using System.Linq;
using GameLib.Common;
using GameLib.Common.DataStructure;
using Gameplay.Data;
using Popup;
using UI.Card;
using UI.Common;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Gameplay
{
    /// <summary>
    /// 牌堆UI管理器。
    /// </summary>
    [RequireComponent(typeof(MoveAction))]
    public class CardPileUIController : CacheablePopupBehaviour
    {
        private ICardCollectionsInfo _info;

        private ShowType _showType;

        [SerializeField]
        private Transform scrollContent;

        [SerializeField]
        private GameObject cardPrefab;

        [SerializeField] private ScrollRect scroll;

        /// <summary>
        /// 初始化UI
        /// </summary>
        /// <param name="info"></param>
        /// <param name="type"></param>
        public void Init(ICardCollectionsInfo info, ShowType type)
        {
            _info = info;
            _showType = type;
            MoveDownUI();
        }

        private void MoveDownUI()
        {
            Transform tr = transform;
            var parentRect = tr.parent.GetComponent<RectTransform>().rect;
            tr.Translate(-new Vector3(0, parentRect.height));
        }

        public override void Show()
        {
            base.Show();
            InitScroll();
            ResetProgressBar();
            PlayUpAnim();
        }
        
        private void InitScroll()
        {
            if (_showType == ShowType.ByOrder)
            {
                InitByOrder();
            }
            else if (_showType == ShowType.Statistics)
            {
                InitByStatistics();
            }
        }

        private void InitByOrder()
        {
            foreach (var card in _info)
            {
                CreateCardObj(card);
            }
        }

        private CardAppearanceSetter CreateCardObj(Data.Card card)
        {
            var cardObj = GameObjectPool.Instance.Get(cardPrefab);
            cardObj.transform.SetParent(scrollContent);
            var setter = cardObj.GetComponent<CardAppearanceSetter>();
            setter.Init(card);
            return setter;
        }

        private void InitByStatistics()
        {
            var counter = new Counter<Data.Card>(_info);
            foreach (var pair in counter.MostCommon())
            {
                var setter = CreateCardObj(pair.Key);
                setter.SetNum((int)pair.Value);
            }
        }


        private void ResetProgressBar()
        {
            scroll.normalizedPosition = new Vector2(0, 1);
        }
        
        private void PlayUpAnim()
        {
            Transform tr = transform;
            var parentRect = tr.parent.GetComponent<RectTransform>().rect;
            var position = tr.position + new Vector3(0, parentRect.height);
            GetComponent<MoveAction>().Move(tr, position);
        }

        public void OnExit()
        {
            CleanCardObj();
            PlayDownAnim();
        }
        
        private void PlayDownAnim()
        {
            Transform tr = transform;
            var parentRect = tr.parent.GetComponent<RectTransform>().rect;
            var position = tr.position - new Vector3(0, parentRect.height);
            GetComponent<MoveAction>().Move(tr, position, Close);
        }

        private void CleanCardObj()
        {
            var allObjs = Enumerable.Range(0, scrollContent.transform.childCount)
                .Select(i => scrollContent.transform.GetChild(i)).ToList();
            foreach (var cardTransform in allObjs)
            {
                cardTransform.SetParent(GameObjectPool.Instance.gameObject.transform);
                GameObjectPool.Instance.Return(cardTransform.gameObject, cardPrefab);
            }
        }
    }

    /// <summary>
    /// 牌组的显示方式。
    /// </summary>
    public enum ShowType
    {
        /// <summary>
        /// 统计显示模式。
        /// </summary>
        Statistics,
        
        /// <summary>
        /// 按顺序显示模式。
        /// </summary>
        ByOrder,
    }
}