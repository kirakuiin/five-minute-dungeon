using Common;
using GameLib.Network;
using Gameplay.Data;
using Save;
using Unity.Netcode;
using UnityEngine;

namespace Gameplay.Broadcaster
{
    /// <summary>
    /// 广播发送者，发送主机信息。
    /// </summary>
    public class BroadcastPublisher : MonoBehaviour
    {
        private readonly TimedBroadcaster<LobbyInfo> _timedBroadcaster = new();

        [SerializeField] private LobbyState currState;

        private bool isRegister;

        private void Start()
        {
            if (!NetworkManager.Singleton.IsServer) return;
            isRegister = true;
            if (currState == LobbyState.InGame)
            {
                InitInGame();
            }
            else
            {
                InitLobby();
            }
        }

        private void InitInGame()
        {
            NetworkManager.Singleton.OnConnectionEvent += OnConnectionEvent;
            BroadInGameInfo();
        }

        private void OnConnectionEvent(NetworkManager mgr, ConnectionEventData @event)
        {
            BroadInGameInfo();
        }

        private void BroadInGameInfo()
        {
            var lobbyName = PlayerSetting.Instance.LobbyName;
            var playerCount = NetworkManager.Singleton.ConnectedClientsIds.Count;
            BroadcastInfo(new LobbyInfo
            {
                lobbyName = lobbyName,
                playerNum = playerCount,
                state = currState
            });
        }

        private void InitLobby()
        {
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
        
        private void OnDestroy()
        {
            if (!isRegister) return;
            _timedBroadcaster.StopBroadcast();
            if (currState == LobbyState.InGame)
            {
                if (NetworkManager.Singleton is null) return;
                NetworkManager.Singleton.OnConnectionEvent -= OnConnectionEvent;
            }
            else
            {
                if (LobbyInfoData.Instance is null) return;
                LobbyInfoData.Instance.OnLobbyInfoChanged -= OnLobbyInfoChanged;
            }
        }
    }
}