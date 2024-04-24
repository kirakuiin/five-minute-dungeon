using System.Linq;
using Data;
using GameLib.Animation;
using GameLib.Common;
using GameLib.Common.DataStructure;
using GameLib.Common.Extension;
using Popup;
using UI.Card;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Gameplay
{
    /// <summary>
    /// 牌堆UI管理器。
    /// </summary>
    public class CardPileUIController : CacheablePopupBehaviour
    {
        private ICardCollectionsInfo _info;

        private ShowType _showType;

        [SerializeField]
        private Transform scrollContent;

        [SerializeField]
        private GameObject cardPrefab;

        [SerializeField] private ScrollRect scroll;

        [SerializeField] private float animTime;

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
            foreach (var card in _info.Reverse())
            {
                CreateCardObj(card);
            }
        }

        private GameObject CreateCardObj(Data.Card card)
        {
            var cardObj = GameObjectPool.Instance.Get(cardPrefab);
            cardObj.transform.SetParent(scrollContent);
            var setter = cardObj.GetComponent<UnplayableCard>();
            setter.Init(card);
            return cardObj;
        }

        private void InitByStatistics()
        {
            var counter = new Counter<Data.Card>(_info);
            foreach (var pair in counter.MostCommon())
            {
                var obj = CreateCardObj(pair.Key);
                obj.GetComponent<CardAppearanceSetter>().SetNum((int)pair.Value);
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
            GetComponent<MoveAction>().MoveTo(tr, position, animTime);
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
            GetComponent<MoveAction>().MoveTo(tr, position, animTime, Close);
        }

        private void CleanCardObj()
        {
            scrollContent.DoSomethingToAllChildren(
                obj => GameObjectPool.Instance.ReturnWithReParent(obj, cardPrefab)
            );
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