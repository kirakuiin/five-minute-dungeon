using System.Collections.Generic;
using GameLib.Network.NGO;
using Gameplay.Data;
using Unity.Netcode;
using UnityEngine;

namespace UI.Lobby
{
    public class LobbyUIController : NetworkBehaviour
    {
        [SerializeField] private GameObject selectionPrefab;

        [SerializeField] private Transform parent;

        private readonly Dictionary<ulong, NetworkObject> _createdPanels = new();

        public override void OnNetworkSpawn()
        {
            if (!IsServer) return;
            LobbyInfoData.Instance.OnPlayerInfoChanged += OnPlayerInfoChanged;
            LobbyInfoData.Instance.OnPlayerJoined += OnPlayerJoined;
            LobbyInfoData.Instance.OnPlayerLeft += OnPlayerLeft;
            InitCreation();
        }

        private void InitCreation()
        {
            foreach (var info in LobbyInfoData.Instance.PlayerInfos.Values)
            {
                CreateSelectionPanel(info);
            }
        }
        
        private void CreateSelectionPanel(PlayerInfo info)
        {
            var networkObj = NetworkObjectPool.Instance.GetNetworkObject(selectionPrefab, Vector3.zero, Quaternion.identity);
            networkObj.SpawnWithOwnership(info.clientID, true);
            networkObj.transform.SetParent(parent);
            _createdPanels[info.clientID] = networkObj;
        }

        private void OnPlayerInfoChanged(PlayerInfo info)
        {
            if (_createdPanels.TryGetValue(info.clientID, out var obj))
            {
                obj.GetComponent<ClassSelectionController>().SetPlayerInfo(info);
            }
        }

        private void OnPlayerJoined(PlayerInfo info)
        {
            CreateSelectionPanel(info);
        }

        private void OnPlayerLeft(ulong clientID)
        {
            if (_createdPanels.TryGetValue(clientID, out var networkObject))
            {
                networkObject.Despawn();
                _createdPanels.Remove(clientID);
            }
        }

        public override void OnNetworkDespawn()
        {
            if (!IsServer) return;
            LobbyInfoData.Instance.OnPlayerInfoChanged -= OnPlayerInfoChanged;
            LobbyInfoData.Instance.OnPlayerJoined -= OnPlayerJoined;
            LobbyInfoData.Instance.OnPlayerLeft -= OnPlayerLeft;
        }
    }
}