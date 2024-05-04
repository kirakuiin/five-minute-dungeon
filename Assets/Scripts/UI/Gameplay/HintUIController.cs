using System.Collections;
using GameLib.Common;
using Popup;
using UI.Common;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Gameplay
{
    public class HintUIController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] public string hintText;

        [SerializeField] private GameObject hintPrefab;
        
        [SerializeField] private float continueTime = 5.0f;

        private Transform _parent;

        private GameObject hintObj;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            hintObj = GameObjectPool.Instance.Get(hintPrefab);
            hintObj.transform.SetParent(PopupManager.Instance.DialogRoot);
            var hintUI = hintObj.GetComponent<HintUI>();
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(GetComponent<RectTransform>(),
                    eventData.position, null, out var pos))
            {
                hintUI.ShowInScreen(hintText, pos);
            }

            hintUI.CallAfterSeconds(continueTime,
                obj => GameObjectPool.Instance.ReturnWithReParent(obj, hintPrefab));
        }

        private void CloseHint()
        {
            if (hintObj is null) return;
            GameObjectPool.Instance.ReturnWithReParent(hintObj, hintPrefab);
            hintObj = null;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            CloseHint();
        }
    }
}