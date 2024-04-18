using System.Collections;
using Gameplay.Data;
using Gameplay.GameState;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace UI.Lobby
{
    public class ReadyTimerUIController : NetworkBehaviour
    {
        [SerializeField] private TMP_Text timerText;

        [SerializeField] private int countDownNum = 5;

        [SerializeField] private LobbyGameState state;

        private int _curCountDown;

        private void Start()
        {
            LobbyInfoData.Instance.OnAllPlayerReady += OnAllPlayerReady;
            gameObject.SetActive(false);
        }

        private void OnAllPlayerReady(bool isAllReady)
        {
            if (isAllReady)
            {
                StartTimerClientRpc();
            }
            else
            {
                StopTimerClientRpc();
            }
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void StopTimerClientRpc()
        {
            StopAllCoroutines();
            timerText.gameObject.SetActive(false);
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void StartTimerClientRpc()
        {
            timerText.gameObject.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(ShowReadyTimer());
        }

        private IEnumerator ShowReadyTimer()
        {
            _curCountDown = countDownNum;
            while (_curCountDown > 0)
            {
                timerText.text = $"{_curCountDown}";
                yield return new WaitForSeconds(1);
                _curCountDown -= 1;
            }
            timerText.gameObject.SetActive(false);

            if (NetworkManager.IsServer)
            {
                state.GoToGamePlay();
            }
        }

        public override void OnNetworkDespawn()
        {
            LobbyInfoData.Instance.OnAllPlayerReady -= OnAllPlayerReady;
        }
    }
}