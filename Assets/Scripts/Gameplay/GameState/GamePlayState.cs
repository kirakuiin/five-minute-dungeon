using System.Collections;
using System.Collections.Generic;
using Common;
using GameLib.Common.Behaviour;
using GameLib.Network.NGO;
using GameLib.Network.NGO.Channel;
using Gameplay.Core;
using Gameplay.Message;
using Gameplay.Progress;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gameplay.GameState
{
    /// <summary>
    /// 游戏中状态。
    /// </summary>
    public class GamePlayState : GameStateBehaviour<GameState>
    {
        [SerializeField] private GamePlayService service;
            
        public override GameState State => GameState.InGame;
        
        /// <summary>
        /// 广播游戏中状态变化。
        /// </summary>
        public NetworkedMessageChannel<GamePlayStateMsg> GameplayState { private set; get; }

        /// <summary>
        /// 当前的游戏中状态。
        /// </summary>
        public GamePlayStateMsg current = GamePlayStateMsg.Create(GamePlayStateEnum.NotStart);

        private NetworkSyncManager Sync => NetworkSyncManager.Instance;

        private void Awake()
        {
            GameplayState = new(NetworkManager.Singleton);
            if (NetworkManager.Singleton.IsServer)
            {
                NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += OnLoadEventCompleted; 
            }
        }

        private void OnLoadEventCompleted(string sceneName, LoadSceneMode mode, List<ulong> clientID, List<ulong> timeouts)
        {
            StartCoroutine(InitServer());
        }

        private IEnumerator InitServer()
        {
            InitContext();
            yield return new WaitUntil(() => NetworkManager.Singleton.IsListening && Sync.HasBeenSyncDone(GamePlayInitStage.InitContext));
            InitController();
            yield return new WaitUntil(() => NetworkManager.Singleton.IsListening && Sync.HasBeenSyncDone(GamePlayInitStage.InitController));
            InitPile();
            yield return new WaitUntil(() => NetworkManager.Singleton.IsListening && Sync.HasBeenSyncDone(GamePlayInitStage.InitPile));
            InitHand();
            yield return new WaitUntil(() => NetworkManager.Singleton.IsListening && Sync.HasBeenSyncDone(GamePlayInitStage.InitHand));
            PublishDone();
            StartPlay();
        }

        private void InitContext()
        {
            GamePlayContext.Instance.InitLevel(GameProgress.Instance.CurrentBoss);
        }

        private void InitController()
        {
            GamePlayContext.Instance.InitPlayerController();
        }

        private void InitPile()
        {
            GamePlayContext.Instance.InitPile();
        }

        private void InitHand()
        {
            GamePlayContext.Instance.InitHand();
        }

        private void PublishDone()
        {
            current = GamePlayStateMsg.Create(GamePlayStateEnum.InitDone);
            GameplayState.Publish(current);
        }

        private void StartPlay()
        {
            service.StartService();
            current = GamePlayStateMsg.Create(GamePlayStateEnum.Running);
            GameplayState.Publish(current);
        }

        protected override void Exit()
        {
            StopAllCoroutines();
            if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsListening)
            {
                NetworkManager.Singleton.SceneManager.OnLoadEventCompleted -= OnLoadEventCompleted;
            }
        }
        
        /// <summary>
        /// 进入结算界面。
        /// </summary>
        public void GoToPostGame()
        {
            current = GamePlayStateMsg.Create(GamePlayStateEnum.End);
            GameplayState.Publish(current);
            SceneLoader.Instance.LoadSceneByNet(SceneDefines.PostGame);
        }
    }
}