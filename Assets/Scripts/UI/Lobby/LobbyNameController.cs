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

        public override void OnNetworkSpawn()
        {
            lobbyName.text = data.LobbyName;
        }
    }
}