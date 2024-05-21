// ReSharper disable once RedundantUsingDirective
using UnityEngine;
using Audio;
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
            var subscriber = ServiceLocator.Instance.Get<ISubscriber<ConnectInfo>>();
            _disposableGroup.Add(subscriber.Subscribe(OnConnectInfo));
            ChangeBgMusic();
        }

        private void ChangeBgMusic()
        {
            BgMusicPlayer.Instance.PlayMainUI();
        }

        protected override void Exit()
        {
            _disposableGroup.Dispose();
        }

        private void OnConnectInfo(ConnectInfo info)
        {
            LockScreenManager.Instance.Unlock();
            var msg = "";
            switch (info.Status)
            {
                case ConnectStatus.Success:
                    break;
                case ConnectStatus.HostEndSession:
                    msg = "主机已关闭。";
                    break;
                case ConnectStatus.StartClientFailed:
                    msg = "连接主机失败。";
                    break;
                case ConnectStatus.ApprovalFailed:
                    msg = "密码错误。";
                    break;
                case ConnectStatus.ServerFull:
                    msg = "服务器人满。";
                    break;
                case ConnectStatus.UserDefined:
                    msg = "仅游戏中断线玩家可以加入。";
                    break;
                default:
                    msg = $"{info}";
                    break;
            }

            if (!string.IsNullOrEmpty(msg))
            {
                InformManager.Instance.CreateInform(msg);
            }
        }
    }
}