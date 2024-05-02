using DG.Tweening;
using GameLib.UI;
using Popup;
using UI.Common;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Card
{
    /// <summary>
    /// 支持卡牌被选中时的一系列特效。
    /// </summary>
    [RequireComponent(typeof(IDrawOrder))]
    [RequireComponent(typeof(PlayableCard))]
    public class CardSelection : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        [SerializeField]
        private float scaleFactor = 1.0f;

        [SerializeField]
        private Vector3 moveOffset = Vector3.zero;

        [SerializeField]
        private bool isEnableRotation;

        [SerializeField]
        private Vector3 rotation = Vector3.zero;

        [SerializeField] private GameObject hintPrefab;

        private const float AnimTime = 0.1f;
        
        private const int MaxOrder = 50;
        
        private IDrawOrder _canvasDrawOrder;

        private Transform _transform;

        private int _originOrder;

        private Vector3 _originScale;

        private Quaternion _originRotation;

        private IPopupDialog hintObj;

        private void Awake()
        {
            _transform = transform;
            _canvasDrawOrder = GetComponent<IDrawOrder>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            SaveOriginInfo();
            if (isEnableRotation)
            {
                transform.DORotate(rotation, AnimTime);
            }
            _canvasDrawOrder.Order = MaxOrder;
            _transform.localScale *= scaleFactor;
            var position = _transform.position;
            transform.DOMove(position + moveOffset, AnimTime).OnComplete(ShowHint);
        }

        private void SaveOriginInfo()
        {
            _originScale = _transform.localScale;
        }

        private void ShowHint()
        {
            var corners = new Vector3[4];
            GetComponent<RectTransform>().GetWorldCorners(corners);
            var hint = GetComponent<ICardData>().CardData.cardHint;
            if (hint is null || hint.Length == 0) return;
            hintObj = PopupManager.Instance.CreatePopup(hintPrefab);
            hintObj.Prefab.GetComponent<HintUI>().ShowAtLeftTop(hint, corners[2]);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            GetComponent<PlayableCard>().ReturnToOriginPos();
            _transform.localScale = _originScale;
            CloseHintObj();
        }

        private void CloseHintObj()
        {
            if (hintObj == null) return;
            hintObj.Close();
            hintObj = null;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            CloseHintObj();
        }
    }
}