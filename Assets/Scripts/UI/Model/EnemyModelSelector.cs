using System;
using Common;
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

        private EnemyCardType _needType;

        private void Awake()
        {
            enabled = false;
        }

        /// <summary>
        /// 是否进入选择模式
        /// </summary>
        /// <param name="type"></param>
        /// <param name="callback"></param>
        public void EnterSelectMode(EnemyCardType type, Action<ulong> callback)
        {
            enabled = true;
            _onSelectCallback = callback;
            _needType = type;
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
            // 处理选择怪物时，怪物被其他方式击毙导致没有合法目标的情况。
            _onSelectCallback?.Invoke((model.Card.type & _needType) > 0 ? model.EnemyID : EnemyIDDefine.Invalid);
        }
    }
}