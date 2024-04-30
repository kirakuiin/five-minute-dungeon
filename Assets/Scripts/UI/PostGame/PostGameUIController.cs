using Common;
using GameLib.Network.NGO;
using TMPro;
using UnityEngine;

namespace UI.PostGame
{
    public class PostGameUIController : MonoBehaviour
    {
        [SerializeField] private TMP_Text resultText;

        [SerializeField] private GameObject winBtn;
        
        [SerializeField] private GameObject loseBtn;
        
        public void GoBackToLobby()
        {
            SceneLoader.Instance.LoadSceneByNet(SceneDefines.LobbyUI); 
        }

        public void Retry()
        {
        }
        
        public void NextChallenge()
        {
        }
    }
}