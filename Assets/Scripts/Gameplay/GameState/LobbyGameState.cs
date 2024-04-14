using System.Collections;
using Common;
using GameLib.Common;
using GameLib.Common.Behaviour;
using GameLib.Network.NGO;
using GameLib.Network.NGO.Channel;
using GameLib.Network.NGO.ConnectionManagement;
using Gameplay.Data;
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
            StartSessionManager();
            SceneLoader.Instance.LoadSceneByNet(SceneDefines.GamePlay);
        }

        private void StartSessionManager()
        {
            var manager = SessionManager<PlayerSessionData>.Instance;
            foreach (var pair in LobbyInfoData.Instance.PlayerInfos)
            {
                var data = manager.GetPlayerData(pair.Key);
                if (data.HasValue)
                {
                    var newData = data.Value;
                    newData.PlayerName = pair.Value.playerName;
                    newData.PlayerClass = pair.Value.selectedClass;
                    manager.UpdatePlayerData(newData.ClientID, newData);
                }
                else
                {
                    Debug.LogError($"会话管理器内存在无效数据(ID={pair.Key}");
                }
            }
            manager.StartSession();
        }

        protected override void Exit()
        {
        }
    }
}