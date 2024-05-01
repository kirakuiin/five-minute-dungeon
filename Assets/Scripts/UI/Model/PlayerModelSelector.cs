using System;
using UI.Common;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Model
{
    /// <summary>
    /// 玩家模型控制器。
    /// </summary>
    [RequireComponent(typeof(PlayerModel))]
    public class PlayerModelSelector : InitComponent, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private GameObject indicator;
        
        [SerializeField] private GameObject selectFlag;
        
        private Action<ulong, bool> _onSelectCallback;

        private bool _canSelectSelf;

        private bool _isSelect;

        private void Awake()
        {
            enabled = false;
        }

        /// <summary>
        /// 是否进入选择模式
        /// </summary>
        /// <param name="canSelectSelf"></param>
        /// <param name="callback"></param>
        public void EnterSelectMode(bool canSelectSelf, Action<ulong, bool> callback)
        {
            enabled = true;
            _onSelectCallback = callback;
            _canSelectSelf = canSelectSelf;
            _isSelect = false;
        }

        /// <summary>
        /// 退出选择模式。
        /// </summary>
        public void ExitSelectMode()
        {
            enabled = false;
            indicator.SetActive(false);
            selectFlag.SetActive(false);
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!CanSelectIt()) return;
            indicator.SetActive(true);
        }

        private bool CanSelectIt()
        {
            return _canSelectSelf || NetworkManager.Singleton.LocalClientId != GetComponent<PlayerModel>().PlayerID;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!CanSelectIt()) return;
            indicator.SetActive(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!CanSelectIt()) return;
            _isSelect = !_isSelect;
            selectFlag.SetActive(_isSelect);
            _onSelectCallback?.Invoke(GetComponent<PlayerModel>().PlayerID, _isSelect);
        }

        public override void Init()
        {
        }
    }
}