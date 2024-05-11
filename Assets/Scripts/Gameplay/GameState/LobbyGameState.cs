using System.Collections;
using Common;
using GameLib.Common;
using GameLib.Common.Behaviour;
using GameLib.Network.NGO;
using GameLib.Network.NGO.Channel;
using GameLib.Network.NGO.ConnectionManagement;
using Gameplay.Data;
using Gameplay.Progress;
using UnityEngine;

namespace Gameplay.GameState
{
    
    /// <summary>
    /// 代表处于游戏房间的游戏状态。
    /// </summary>
    public class LobbyGameState : GameStateBehaviour<GameState>
    {
        public override GameState State { get; } = GameState.Lobby;

        private readonly DisposableGroup _disposableGroup = new();

        protected override void Enter()
        {
            InitEvent();
        }

        private void InitEvent()
        {
            var sub = ServiceLocator.Instance.Get<ISubscriber<ConnectStatus>>();
            _disposableGroup.Add(sub.Subscribe(OnConnectStatus));
        }

        private void OnConnectStatus(ConnectStatus status)
        {
            Debug.Log($"连接状态转为 {status}");
            if (status == ConnectStatus.HostEndSession)
            {
                SceneLoader.Instance.LoadScene(SceneDefines.MainUI);
            }
        }

        /// <summary>
        /// 返回主菜单。
        /// </summary>
        public void GoBackToMain()
        {
            ConnectionManager.Instance.UserRequestShutdown();
        }
        
        /// <summary>
        /// 进入玩法界面。
        /// </summary>
        public void GoToGamePlay()
        {
            SessionManager<PlayerSessionData>.Instance.StartSession();
            SceneLoader.Instance.LoadSceneByNet(SceneDefines.GamePlay);
        }

        public void ResetProgress()
        {
            GameProgress.Instance.Reset();
        }

        protected override void Exit()
        {
        }
    }
}