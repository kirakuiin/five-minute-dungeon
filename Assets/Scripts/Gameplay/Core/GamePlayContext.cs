﻿using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Data.Animation;
using Data.Check;
using Data.Instruction;
using GameLib.Common;
using GameLib.Network.NGO;
using Gameplay.Data;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gameplay.Core
{
    /// <summary>
    /// 负责提供游戏运行时的上下文环境。
    /// </summary>
    public class GamePlayContext : NetworkSingleton<GamePlayContext>, ICmdContext, IRuntimeContext
    {
        [SerializeField]
        private GameObject playerControllerPrefab; 
        
        private readonly Dictionary<ulong, PlayerController> _playerControllers = new();

        public event Action<ConnectionEvent, ulong> OnConnectionEvent;

        /// <summary>
        /// 初始化关卡。
        /// </summary>
        public void InitLevel(Boss boss)
        {
            GetComponent<LevelController>().Setup(PlayerCount, boss);
        }

        protected override void Awake()
        {
            base.Awake();
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += OnLoadEventCompleted;
            if (NetworkManager.IsServer)
            {
                NetworkManager.Singleton.OnConnectionEvent += OnNetConnectionEvent;
                NetworkManager.Singleton.SceneManager.OnUnload += OnUnload;
            }
        }

        private void OnUnload(ulong clientId, string sceneName, AsyncOperation operation)
        {
            foreach (var controller in _playerControllers.Values)
            {
                controller.GetComponent<NetworkObject>().Despawn();
            }
        }

        private void OnLoadEventCompleted(string sceneName, LoadSceneMode mode, List<ulong> clientID, List<ulong> timeouts)
        {
            RegisterPlayerControllerEvent(PlayerCount);
        }

        private void OnNetConnectionEvent(NetworkManager mgr, ConnectionEventData data)
        {
            if (data.ClientId == NetworkManager.ServerClientId) return;
            if (data.EventType == ConnectionEvent.ClientDisconnected&& _playerControllers.ContainsKey(data.ClientId))
            {
                _playerControllers.Remove(data.ClientId);
            }
            NetEventRpc(data.EventType, data.ClientId);
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void NetEventRpc(ConnectionEvent type, ulong clientID)
        {
            OnConnectionEvent?.Invoke(type, clientID);
        }

        public void InitPlayerController()
        {
            foreach (var clientID in GetAllClientIDs())
            {
                if (_playerControllers.ContainsKey(clientID)) continue;
                var obj = NetworkObject.InstantiateAndSpawn(playerControllerPrefab, NetworkManager, clientID);
                obj.transform.SetParent(transform);
            }
        }

        private void RegisterPlayerControllerEvent(int needCount)
        {
            LocalSyncManager.Create();
            if (NetworkManager.IsClient)
            {
                LocalSyncManager.Instance.AddSyncEvent(LocalSyncInitStage.InitPlayerController, needCount, OnPlayerControllerInitDone);
                LocalSyncManager.Instance.AddSyncEvent(LocalSyncInitStage.InitPile, needCount, OnInitPileDone);
            }
        }

        private void OnPlayerControllerInitDone()
        {
            BuildPlayerMap();
            NetworkSyncManager.Instance.AddSyncEvent(GamePlayInitStage.InitController);
        }
        
        private void BuildPlayerMap()
        {
            for (var i = 0; i < transform.childCount; ++i)
            {
                if (transform.GetChild(i).TryGetComponent<PlayerController>(out var controller))
                {
                    _playerControllers[controller.ClientID] = controller;
                }
            }
        }

        private void OnInitPileDone()
        {
            NetworkSyncManager.Instance.AddSyncEvent(GamePlayInitStage.InitPile);
        }
        
        /// <summary>
        /// 玩家数量。
        /// </summary>
        public int PlayerCount => GetAllClientIDs().Count();

        public int InitHandNum => GameRule.GetInitHandNum(PlayerCount);

        public IPlayerController GetPlayerController(ulong clientID)
        {
            return _playerControllers[clientID];
        }

        public IPlayerController GetPlayerController()
        {
            var clientID = NetworkManager.LocalClientId;
            return GetPlayerController(clientID);
        }

        public IPlayerRuntimeInfo GetPlayerRuntimeInfo(ulong clientID)
        {
            return _playerControllers[clientID];
        }

        public IPlayerRuntimeInfo GetPlayerRuntimeInfo()
        {
            var clientID = NetworkManager.LocalClientId;
            return GetPlayerRuntimeInfo(clientID);
        }

        public ILevelRuntimeInfo GetLevelRuntimeInfo()
        {
            return GetComponent<ILevelRuntimeInfo>();
        }

        public ITimeRuntimeInfo GetTimeRuntimeInfo()
        {
            return GetComponent<ITimeRuntimeInfo>();
        }

        public IEnumerable<ulong> GetAllClientIDs()
        {
            return NetworkManager.ConnectedClientsIds;
        }

        public ulong GetServerID()
        {
            return NetworkManager.ServerClientId;
        }

        public ILevelController GetLevelController()
        {
            return GetComponent<ILevelController>();
        }

        public ITimeController GetTimeController()
        {
            return GetComponent<ITimeController>();
        }

        public IBehaveController GetBehaveController()
        {
            return GetComponent<IBehaveController>();
        }

        /// <summary>
        /// 初始化抽牌堆。
        /// </summary>
        public void InitPile()
        {
            var data = (from clientID in NetworkManager.ConnectedClientsIds
                // ReSharper disable once PossibleInvalidOperationException
                select SessionManager<PlayerSessionData>.Instance.GetPlayerData(clientID).Value).ToList();
            
            var enumerator = new DeckGenerator(data.Select(obj => obj.PlayerClass)).GetDeckEnumerator();
            
            for(var i = 0; i < NetworkManager.ConnectedClientsIds.Count; ++i)
            {
                enumerator.MoveNext();
                var controller = _playerControllers[NetworkManager.ConnectedClientsIds[i]];
                if (enumerator.Current == null) continue;
                var initDraw = enumerator.Current.ToList();
                controller.Init(initDraw.Skip(InitHandNum), data[i].PlayerClass, data[i].PlayerName);
                controller.SetFirstHand(initDraw.Take(InitHandNum));
            }
        }

        public override void OnNetworkDespawn()
        {
            if (!NetworkManager) return;
            NetworkManager.SceneManager.OnLoadEventCompleted -= OnLoadEventCompleted;
            if (NetworkManager.IsServer)
            {
                NetworkManager.Singleton.SceneManager.OnUnload -= OnUnload;
                NetworkManager.Singleton.OnConnectionEvent -= OnNetConnectionEvent;
            }
        }

        public void Reconnect()
        {
            GetTimeController().Stop();
            RegisterReconnectEventRpc();
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void RegisterReconnectEventRpc()
        {
            RegisterPlayerControllerEvent(1);
            NetworkSyncManager.Instance.AddSyncEvent(GamePlayInitStage.ReconnectEvent);
        }
        
        /// <summary>
        /// 重新初始化客户端。
        /// </summary>
        /// <param name="clientID"></param>
        public void ReInit(ulong clientID)
        {
            var controller = _playerControllers[clientID];
            var data = SessionManager<PlayerSessionData>.Instance.GetPlayerData(clientID);
            if (!data.HasValue) return;
            controller.Init(data.Value.DrawData, data.Value.PlayerClass, data.Value.PlayerName);
            controller.SetHandAndDiscard(data.Value.HandData, data.Value.DiscardData);
        }

    }
}