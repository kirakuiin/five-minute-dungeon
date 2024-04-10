using Common;
using GameLib.Network;
using Gameplay.Data;
using Unity.Netcode;
using UnityEngine;

namespace Gameplay.Broadcaster
{
    /// <summary>
    /// 广播发送者，发送主机信息。
    /// </summary>
    public class BroadcastPublisher : NetworkBehaviour
    {
        private readonly TimedBroadcaster<LobbyInfo> _timedBroadcaster = new();

        [SerializeField] private LobbyState currState;

        public override void OnNetworkSpawn()
        {
            if (!NetworkManager.IsServer) return;
            var data = LobbyInfoData.Instance;
            data.OnLobbyInfoChanged += OnLobbyInfoChanged;
            BroadcastInfo(new LobbyInfo()
            {
                lobbyName = data.LobbyName,
                playerNum = data.PlayerCount,
                state = currState
            });
        }

        private void OnLobbyInfoChanged(LobbyInfo info)
        {
            BroadcastInfo(info);
        }

        private void BroadcastInfo(LobbyInfo info)
        {
            _timedBroadcaster.StopBroadcast();
            _timedBroadcaster.StartBroadcast(info);
        }
        
        public override void OnNetworkDespawn()
        {
            if (!NetworkManager.IsServer) return;
            _timedBroadcaster.StopBroadcast();
            LobbyInfoData.Instance.OnLobbyInfoChanged -= OnLobbyInfoChanged;
        }
    }
}