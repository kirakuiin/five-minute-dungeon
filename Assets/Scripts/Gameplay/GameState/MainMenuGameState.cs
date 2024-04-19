using System;
using GameLib.Common;
using GameLib.Common.Behaviour;
using GameLib.Network.NGO.Channel;
using GameLib.Network.NGO.ConnectionManagement;
using Common;
using GameLib.Network;
using GameLib.Network.NGO;
using Gameplay.Connection;
using Popup;

namespace Gameplay.GameState
{
    /// <summary>
    /// 代表处于主界面的游戏状态。
    /// </summary>
    public class MainMenuGameState : GameStateBehaviour<GameState>
    {
        public override GameState State { get; } = GameState.MainMenu;

        private readonly DisposableGroup _disposableGroup = new();

        public void OnExitApplication()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        
        /// <summary>
        /// 加入主机。
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="password"></param>
        public void JoinGame(string ipAddress, string password)
        {
            try
            {
                LockScreenManager.Instance.Lock("连接中...");
                RebuildConnectionMethod(ipAddress, password);
                ConnectionManager.Instance.StartClient();
            }
            catch (IPParseException)
            {
                LockScreenManager.Instance.Unlock();
                InformManager.Instance.CreateInform($"IP地址无效: {ipAddress}。");
            }
        }

        private void RebuildConnectionMethod(string ipAddress, string password)
        {
            var connectionMethod = new IPPassConnectionMethod(
                Address.GetIPEndPoint(ipAddress, NetworkDefines.Port),
                password
            );
            foreach (var comp in ConnectionManager.Instance.GetStatesByInterface<IConnectionResettable>())
            {
                comp.SetConnectionMethod(connectionMethod);
            }
        }

        protected override void Enter()
        {
            var subscriber = ServiceLocator.Instance.Get<ISubscriber<ConnectStatus>>();
            _disposableGroup.Add(subscriber.Subscribe(OnConnectStatus));
        }

        protected override void Exit()
        {
            _disposableGroup.Dispose();
        }

        private void OnConnectStatus(ConnectStatus status)
        {
            LockScreenManager.Instance.Unlock();
            string info = "";
            switch (status)
            {
                case ConnectStatus.Success:
                    break;
                case ConnectStatus.StartClientFailed:
                    info = "连接主机失败。";
                    break;
                case ConnectStatus.ApprovalFailed:
                    info = "密码错误。";
                    break;
                case ConnectStatus.ServerFull:
                    info = "服务器人满。";
                    break;
                default:
                    info = $"{status}";
                    break;
            }
            if (!String.IsNullOrEmpty(info))
            {
                InformManager.Instance.CreateInform(info);
            }
        }
    }
}