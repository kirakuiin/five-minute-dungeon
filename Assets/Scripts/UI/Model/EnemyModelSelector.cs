using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Model
{
    [RequireComponent(typeof(EnemyModel))]
    public class EnemyModelSelector: MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject indicator;
        
        private Action<ulong> _onSelectCallback;

        private void Awake()
        {
            enabled = false;
        }

        /// <summary>
        /// 是否进入选择模式
        /// </summary>
        /// <param name="callback"></param>
        public void EnterSelectMode(Action<ulong> callback)
        {
            enabled = true;
            _onSelectCallback = callback;
        }

        /// <summary>
        /// 退出选择模式。
        /// </summary>
        public void ExitSelectMode()
        {
            enabled = false;
            indicator.SetActive(false);
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            indicator.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            indicator.SetActive(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var model = GetComponent<EnemyModel>();
            _onSelectCallback?.Invoke(model.EnemyID);
        }
    }
}