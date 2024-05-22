using Common;
using Data;
using GameLib.Common;
using GameLib.Common.Behaviour;
using GameLib.Network;
using GameLib.Network.NGO;
using GameLib.Network.NGO.Channel;
using GameLib.Network.NGO.ConnectionManagement;
using Gameplay.Connection;
using Save;
using Unity.Netcode;
using UnityEngine;
using Screen = UnityEngine.Device.Screen;

namespace Gameplay.GameState
{
    public class StartupState : GameStateBehaviour<GameState>
    {
        public override GameState State { get; } = GameState.Startup;

        protected override void Enter()
        {
            InitBasicConfig();
            InitDataBase();
            InitPublisher();
            InitConnection();
            SceneLoader.Instance.LoadScene(SceneDefines.MainUI);
        }

        private void InitBasicConfig()
        {
            NetworkManager.Singleton.NetworkConfig.SpawnTimeout = 5.0f;
            Screen.SetResolution(GameConfig.ScreenWidth, GameConfig.ScreenHeight, PlayerSetting.Instance.IsFullScreen);
        }

        private void InitDataBase()
        {
            DataService.Create();
        }

        private void InitPublisher()
        {
            ServiceLocator.Instance.Register<IPublisher<ConnectInfo>>(new MessageChannel<ConnectInfo>());
        }

        private void InitConnection()
        {
            var endpoint = Address.GetIPEndPoint(Address.GetUniversalIPAddress().ToString(), NetworkDefines.Port);
            var connectMethod = new IPPassConnectionMethod(endpoint, "");
            ConnectionManager.Instance.AddState<StartHostingState>(new StartHosting(connectMethod));
            ConnectionManager.Instance.AddState<ClientReconnectingState>(new ClientReconnecting(connectMethod));
            ConnectionManager.Instance.AddState<ClientConnectingState>(new ClientConnecting(connectMethod));
            ConnectionManager.Instance.AddState<HostingState>(new Hosting());
            ConnectionManager.Instance.AddState<OfflineState>(new Offline());
            ConnectionManager.Instance.AddState<ClientConnectedState>(new ClientConnected());
        }
    }
}