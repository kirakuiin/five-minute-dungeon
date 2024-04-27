using System;
using Data;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Model
{
    [RequireComponent(typeof(EnemyModel))]
    public class EnemyModelSelector: MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private EventTrigger _trigger;

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
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("鼠标进入");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("鼠标离开");
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var model = GetComponent<EnemyModel>();
            _onSelectCallback?.Invoke(model.EnemyID);
        }
    }
}