using Common;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Card
{
    /// <summary>
    /// 用于执行卡牌在各个区域打出时的触发效果。
    /// </summary>
    [RequireComponent(typeof(PlayableCard))]
    public class CardTrigger : MonoBehaviour, IEndDragHandler, IBeginDragHandler
    {
        private string _curZoneTag;

        private PlayableCard _card;
        
        public void Awake()
        {
            _card = GetComponent<PlayableCard>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            _curZoneTag = other.tag;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            _curZoneTag = "";
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _curZoneTag = "";
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (TagDefines.IsPlayArea(_curZoneTag))
            {
                _card.PlayCard();
            }
            else if (TagDefines.IsDiscardArea(_curZoneTag))
            {
                _card.DiscardCard();
            }
            else
            {
                _card.ReturnToOriginPos();
            }
        }
    }
}