using System;
using Gameplay.GameState;
using UnityEngine;
using TMPro;

namespace UI.Menu
{
    public class SearchJoinController : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text lobbyName;

        [SerializeField]
        private TMP_InputField password;

        [SerializeField]
        private MainMenuGameState state;

        private string _ipAddress;

        /// <summary>
        /// 设置连接属性。
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="targetName"></param>
        public void Init(string ip, string targetName)
        {
            gameObject.SetActive(true);
            lobbyName.text = targetName;
            _ipAddress = ip;
        }

        public void OnConfirm()
        {
            state.JoinGame(_ipAddress, password.text);
            gameObject.SetActive(false);
        }

        public void OnCancel()
        {
            gameObject.SetActive(false);
        }

        private void OnBecameInvisible()
        {
            gameObject.SetActive(false);
        }
    }
}