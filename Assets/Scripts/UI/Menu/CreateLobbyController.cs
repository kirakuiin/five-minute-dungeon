using GameLib.Network.NGO.ConnectionManagement;
using Popup;
using Save;
using TMPro;
using UnityEngine;

namespace UI.Menu
{
    public class CreateLobbyController : CacheablePopupBehaviour
    {
        [SerializeField]
        private TMP_InputField lobbyName;

        [SerializeField]
        private TMP_InputField password;

        private void Start()
        {
            lobbyName.text = PlayerSetting.Instance.LobbyName;
            password.text = PlayerSetting.Instance.LobbyPassword;
        }

        public void OnConfirm()
        {
            SaveLobbyInfo();
            ConnectionManager.Instance.StartHost();
        }

        private void SaveLobbyInfo()
        {
            PlayerSetting.Instance.LobbyName = lobbyName.text;
            PlayerSetting.Instance.LobbyPassword = password.text;
        }
    }
}