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
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(GetComponent<RectTransform>(),
                    eventData.position, null, out var pos))
            {
                hintObj.GetComponent<HintUI>().ShowInScreen(hintText, pos);
            }

            StartCoroutine(AutoDisappear());
        }

        private IEnumerator AutoDisappear()
        {
            yield return new WaitForSeconds(continueTime);
            CloseHint();
        }

        private void CloseHint()
        {
            if (hintObj is null) return;
            StopAllCoroutines();
            GameObjectPool.Instance.ReturnWithReParent(hintObj, hintPrefab);
            hintObj = null;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            CloseHint();
        }
    }
}