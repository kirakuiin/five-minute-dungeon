using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Data;
using GameLib.Network.NGO;
using Save;
using Unity.Collections;
using Unity.Netcode;

namespace Gameplay.Data
{
    /// <summary>
    /// 房间信息。
    /// </summary>
    [Serializable]
    public struct LobbyInfo : IEquatable<LobbyInfo>, INetworkSerializeByMemcpy
    {
        public FixedString32Bytes lobbyName;
        public int playerNum;
        public LobbyState state;

        public bool Equals(LobbyInfo other)
        {
            return lobbyName == other.lobbyName && playerNum == other.playerNum && state == other.state;
        }
    }
    
    /// <summary>
    /// 玩家信息。
    /// </summary>
    public struct PlayerInfo : IEquatable<PlayerInfo>, INetworkSerializable
    {
        /// <summary>
        /// 玩家客户端ID。
        /// </summary>
        public ulong clientID;

        /// <summary>
        /// 玩家用户名。
        /// </summary>
        public string playerName;

        /// <summary>
        /// 玩家选择的职业。
        /// </summary>
        public Class selectedClass;

        /// <summary>
        /// 玩家是否就绪。
        /// </summary>
        public bool isReady;

        public PlayerInfo(ulong clientID, string playerName="")
        {
            this.clientID = clientID;
            this.playerName = playerName;
            selectedClass = Class.Barbarian;
            isReady = false;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref clientID);
            serializer.SerializeValue(ref playerName);
            serializer.SerializeValue(ref selectedClass);
            serializer.SerializeValue(ref isReady);
        }

        public bool Equals(PlayerInfo obj)
        {
            return clientID == obj.clientID
                   && playerName == obj.playerName
                   && selectedClass == obj.selectedClass
                   && isReady == obj.isReady;
        }

        public override string ToString()
        {
            return $"{playerName}({clientID})[职业={selectedClass}]目前{(isReady ? "就绪" : "配置中")}";
        }
    }
    
    /// <summary>
    /// 管理当前房间地数据
    /// </summary>
    public class LobbyInfoData : NetworkSingleton<LobbyInfoData>
    {
        private readonly NetworkVariable<FixedString32Bytes> _lobbyName = new();

        private readonly Dictionary<ulong, PlayerInfo> _playerInfos = new();

        /// <summary>
        /// 返回玩家信息列表。
        /// </summary>
        public IReadOnlyDictionary<ulong, PlayerInfo> PlayerInfos => _playerInfos;
        
        /// <summary>
        /// 获得房间名。
        /// </summary>
        public string LobbyName
        {
            private set => _lobbyName.Value = value;
            get => _lobbyName.Value.ToString();
        }

        /// <summary>
        /// 当前玩家数。
        /// </summary>
        public int PlayerCount => _playerInfos.Count;

        /// <summary>
        /// 房间信息发生变动时触发。
        /// </summary>
        public event Action<LobbyInfo> OnLobbyInfoChanged;

        /// <summary>
        /// 玩家信息发生变化时触发。
        /// </summary>
        public event Action<PlayerInfo> OnPlayerInfoChanged;

        /// <summary>
        /// 当玩家加入时触发。
        /// </summary>
        public event Action<PlayerInfo> OnPlayerJoined;

        /// <summary>
        /// 当玩家离开时触发。
        /// </summary>
        public event Action<ulong> OnPlayerLeft;

        /// <summary>
        /// 全部玩家准备时触发。
        /// </summary>
        public event Action<bool> OnAllPlayerReady;

        protected override void OnSynchronize<T>(ref BufferSerializer<T> serializer)
        {
            if (serializer.IsWriter)
            {
                var writer = serializer.GetFastBufferWriter();
                writer.WriteValueSafe(_playerInfos.Count);
                foreach (var pair in _playerInfos)
                {
                    writer.WriteValueSafe(pair.Value);
                }
            }
            else
            {
                _playerInfos.Clear();
                var reader = serializer.GetFastBufferReader();
                reader.ReadValueSafe(out int count);
                for (int i = 0; i < count; ++i)
                {
                    reader.ReadValueSafe(out PlayerInfo info);
                    _playerInfos[info.clientID] = info;
                }
            }
        }

        public override void OnNetworkSpawn()
        {
            if (!IsServer) return;
            
            InitLobbyName();
            InitPlayerInfo();
            InitListening();
        }

        private void InitLobbyName()
        {
            LobbyName = PlayerSetting.Instance.LobbyName;
        }

        private void InitPlayerInfo()
        {
            var clientID = NetworkManager.Singleton.LocalClientId;
            _playerInfos[clientID] = new PlayerInfo(clientID, PlayerSetting.Instance.PlayerName);
            InvokeLobbyChangeOnServer();
            InvokePlayerJoinedClientRpc(_playerInfos[clientID]);
        }
        
        private void InitListening()
        {
            var manager = NetworkManager.Singleton;
            manager.OnClientConnectedCallback += OnClientConnectedCallback;
            manager.OnClientDisconnectCallback += OnClientDisconnectCallback;
        }

        private void OnClientConnectedCallback(ulong clientID)
        {
            _playerInfos[clientID] = new PlayerInfo(clientID);
            InvokeLobbyChangeOnServer();
            InvokePlayerJoinedClientRpc(_playerInfos[clientID]);
            InvokeReadyInfo();
        }

        private void InvokeLobbyChangeOnServer()
        {
            OnLobbyInfoChanged?.Invoke(new LobbyInfo()
            {
                lobbyName = LobbyName,
                playerNum = PlayerCount,
                state = LobbyState.WaitForPlayer,
            });
        }

        [ClientRpc]
        private void InvokePlayerJoinedClientRpc(PlayerInfo info)
        {
            if (!IsServer)
            {
                _playerInfos[info.clientID] = info;
            }
            OnPlayerJoined?.Invoke(info);
        }

        private void OnClientDisconnectCallback(ulong clientID)
        {
            if (!_playerInfos.ContainsKey(clientID)) return;
            _playerInfos.Remove(clientID);
            InvokeLobbyChangeOnServer();
            InvokePlayerLeftClientRpc(clientID);
            InvokeReadyInfo();
        }

        [ClientRpc]
        private void InvokePlayerLeftClientRpc(ulong clientID)
        {
            if (!IsServer)
            {
                _playerInfos.Remove(clientID);
            }
            OnPlayerLeft?.Invoke(clientID);
        }

        public override void OnNetworkDespawn()
        {
            if (IsServer)
            {
                NetworkManager.OnClientConnectedCallback -= OnClientConnectedCallback;
                NetworkManager.OnClientDisconnectCallback -= OnClientDisconnectCallback;
            }
            base.OnNetworkDespawn();
        }

        /// <summary>
        /// 设置玩家名称。
        /// </summary>
        /// <param name="playerName"></param>
        public void SetPlayerName(string playerName)
        {
            SetPlayerNameServerRpc(playerName);
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetPlayerNameServerRpc(string playerName, ServerRpcParams rpcParams=default)
        {
            var clientID = rpcParams.Receive.SenderClientId;
            if (_playerInfos.TryGetValue(clientID, out var curInfo))
            {
                curInfo.playerName = playerName;
                UpdatePlayerInfo(curInfo);
            }
        }

        private void UpdatePlayerInfo(PlayerInfo curInfo)
        {
            var clientID = curInfo.clientID;
            if (!_playerInfos[clientID].Equals(curInfo))
            {
                _playerInfos[clientID] = curInfo;
                InvokePlayerInfoChangedClientRpc(curInfo);
            }
        }
        
        [ClientRpc]
        private void InvokePlayerInfoChangedClientRpc(PlayerInfo info)
        {
            if (!IsServer)
            {
                _playerInfos[info.clientID] = info;
            }
            OnPlayerInfoChanged?.Invoke(info);
        }

        /// <summary>
        /// 设置玩家选择的职业。
        /// </summary>
        /// <param name="classType"></param>
        public void SetPlayerClass(Class classType)
        {
            SetPlayerClassServerRpc(classType);
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetPlayerClassServerRpc(Class classType, ServerRpcParams rpcParams=default)
        {
            var clientID = rpcParams.Receive.SenderClientId;
            if (_playerInfos.TryGetValue(clientID, out var curInfo))
            {
                curInfo.selectedClass = classType;
                UpdatePlayerInfo(curInfo);
            }
        }
        
        /// <summary>
        /// 设置玩家准备状态。
        /// </summary>
        /// <param name="isReady"></param>
        public void SetPlayerReady(bool isReady)
        {
            SetPlayerReadyServerRpc(isReady);
        }
        
        [ServerRpc(RequireOwnership = false)]
        private void SetPlayerReadyServerRpc(bool isReady, ServerRpcParams rpcParams=default)
        {
            var clientID = rpcParams.Receive.SenderClientId;
            if (_playerInfos.TryGetValue(clientID, out var curInfo))
            {
                curInfo.isReady = isReady;
                UpdatePlayerInfo(curInfo);
                InvokeReadyInfo();
            }
        }

        private void InvokeReadyInfo()
        {
            var isAllReady = (from info in _playerInfos.Values
                select info).All(info => info.isReady);
            OnAllPlayerReady?.Invoke(isAllReady);
        }
    }
}