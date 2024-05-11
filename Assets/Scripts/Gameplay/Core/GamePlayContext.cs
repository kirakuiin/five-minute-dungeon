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
            RegisterPlayerControllerEvent();
            NetworkSyncManager.Instance.AddSyncEvent(GamePlayInitStage.InitContext);
        }

        public void InitPlayerController()
        {
            foreach (var clientID in GetAllClientIDs())
            {
                var obj = NetworkObject.InstantiateAndSpawn(playerControllerPrefab, NetworkManager, clientID);
                obj.transform.SetParent(transform);
            }
        }

        private void RegisterPlayerControllerEvent()
        {
            if (NetworkManager.IsClient)
            {
                LocalSyncManager.Instance.AddSyncEvent(LocalSyncInitStage.InitPlayerController, PlayerCount, OnPlayerControllerInitDone);
                LocalSyncManager.Instance.AddSyncEvent(LocalSyncInitStage.InitPile, PlayerCount, OnInitPileDone);
                LocalSyncManager.Instance.AddSyncEvent(LocalSyncInitStage.InitHand, PlayerCount, OnInitHandDone);
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

        private void OnInitHandDone()
        {
            NetworkSyncManager.Instance.AddSyncEvent(GamePlayInitStage.InitHand);
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
                controller.Init(enumerator.Current, data[i].PlayerClass, data[i].PlayerName);
            }
        }

        /// <summary>
        /// 初始化手牌。
        /// </summary>
        public void InitHand()
        {
            foreach (var clientID in GetAllClientIDs())
            {
                var controller = GetPlayerController(clientID);
                controller.Draw(InitHandNum);
            }
        }

        public override void OnNetworkDespawn()
        {
            if (!NetworkManager) return;
            NetworkManager.SceneManager.OnLoadEventCompleted -= OnLoadEventCompleted;
            if (NetworkManager.IsServer)
            {
                NetworkManager.Singleton.SceneManager.OnUnload -= OnUnload;
            }
        }
    }
}