using UnityEngine;
using UnityEngine.Windows.WebCam;

namespace UI.Menu
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject createLobbyUI;

        [SerializeField]
        private GameObject joinLobbyUI;

        public void OnCreateLobby()
        {
            createLobbyUI.SetActive(true);
            joinLobbyUI.SetActive(false);
        }

        public void OnJoinLobby()
        {
            createLobbyUI.SetActive(false);
            joinLobbyUI.SetActive(true);
        }
    }
}