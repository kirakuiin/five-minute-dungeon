using System;
using Popup;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu
{
    public class JoinLobbyController : CacheablePopupBehaviour
    {
        [Tooltip("IP直连窗口")]
        [SerializeField]
        private GameObject ipInput;

        [Tooltip("搜索房间窗口")]
        [SerializeField]
        private GameObject searchLobby;

        [Tooltip("ip直连按钮")]
        [SerializeField]
        private Toggle ipToggle;

        private void Start()
        {
            ipToggle.isOn = true;
        }

        public void OnIPToggleChanged(bool isOn)
        {
            ipInput.SetActive(isOn);
        }

        public void OnSearchToggleChanged(bool isOn)
        {
            searchLobby.SetActive(isOn);
        }
    }
}