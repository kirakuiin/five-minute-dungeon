using GameLib.Network.NGO;
using Gameplay.Data;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace UI.Lobby
{
    public class LobbyNameController : NetworkBehaviour
    {
        [SerializeField]
        private TMP_Text lobbyName;

        [SerializeField]
        private LobbyInfoData data;

        [SerializeField]
        private GameObject resetBtn;

        public override void OnNetworkSpawn()
        {
            lobbyName.text = data.LobbyName;
            resetBtn.SetActive(IsServer);
        }
    }
}