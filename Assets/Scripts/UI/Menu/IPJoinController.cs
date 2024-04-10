using Gameplay.GameState;
using TMPro;
using UnityEngine;

namespace UI.Menu
{
    public class IPJoinController : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField ip;

        [SerializeField]
        private TMP_InputField password;

        [SerializeField]
        private MainMenuGameState state;

        public void OnConfirm()
        {
            state.JoinGame(ip.text, password.text);
        }
    }
}