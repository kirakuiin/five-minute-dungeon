using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Card
{
    /// <summary>
    /// 手牌选择器。
    /// </summary>
    public class HandCardSelector : MonoBehaviour, IPointerDownHandler
    {
        private bool _isSelected;
        
        public void OnPointerDown(PointerEventData eventData)
        {
            _isSelected = !_isSelected;
            GetComponent<CardAppearanceSetter>().SetSelected(_isSelected);
            GetComponent<PlayableCard>().SelectCard(_isSelected);
        }

        /// <summary>
        /// 开启。
        /// </summary>
        public void EnableIt()
        {
            _isSelected = false;
            enabled = true;
        }

        /// <summary>
        /// 禁止。
        /// </summary>
        public void DisableIt()
        {
            GetComponent<CardAppearanceSetter>().SetSelected(false);
            enabled = false;
        }
    }
}