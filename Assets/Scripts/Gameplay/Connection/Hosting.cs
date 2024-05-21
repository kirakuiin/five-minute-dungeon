using Common;
using GameLib.Common;
using GameLib.Common.Behaviour;
using GameLib.Network.NGO;
using GameLib.Network.NGO.ConnectionManagement;
using Gameplay.Data;
using Save;
using Unity.Netcode;
using UnityEngine;

namespace Gameplay.Connection
{
    public class Hosting : HostingState
    {
        public override void Enter()
        {
            base.Enter();
            SceneLoader.Instance.LoadSceneByNet(SceneDefines.LobbyUI);
        }

        public override void Exit()
        {
            SessionManager<PlayerSessionData>.Instance.ClearAllData();
            SceneLoader.Instance.LoadScene(SceneDefines.MainUI);
            base.Exit();
        }

        public override void OnClientDisconnected(ulong clientID)
        {
            if (clientID != NetManager.LocalClientId)
            {
                SessionManager<PlayerSessionData>.Instance.DisconnectClient(clientID);
            }
        }

        protected override void SetResponse(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
        {
            var info = GetConnectInfo(request);
            if (info.Status == ConnectStatus.Success)
            {
                RegisterPlayerSessionData(request);
                response.CreatePlayerObject = false;
                response.Approved = true;
            }
            else
            {
                response.Reason = JsonUtility.ToJson(info);
                response.Approved = false;
            }
        }

        private void RegisterPlayerSessionData(NetworkManager.ConnectionApprovalRequest request)
        {
            var clientID = request.ClientNetworkId;
            var payload = SerializeTool.Deserialize<Protocol.ConnectionPayload>(request.Payload);
            SessionManager<PlayerSessionData>.Instance.SetupPlayerData(clientID, payload.playerGuid,
                new PlayerSessionData() {ClientID = clientID, PlayerID = payload.playerGuid, IsConnected = true}
            );
            Debug.Log($"保存玩家{payload.playerGuid}的会话数据。");
        }

        protected override ConnectInfo GetConnectInfo(NetworkManager.ConnectionApprovalRequest request)
        {
            var payload = SerializeTool.Deserialize<Protocol.ConnectionPayload>(request.Payload);
            if (!IsInGame() && !IsInLobby())
            {
                return ConnectInfo.Create(ConnectStatus.HostEndSession);
            }
            if (NetManager.ConnectedClientsIds.Count >= ConnManager.config.maxConnectedPlayerNum)
            {
                return ConnectInfo.Create(ConnectStatus.ServerFull);
            }
            if (payload.password != PlayerSetting.Instance.LobbyPassword)
            {
                return ConnectInfo.Create(ConnectStatus.ApprovalFailed);
            }
            if (SessionManager<PlayerSessionData>.Instance.IsDuplicateConnection(payload.playerGuid))
            {
                return ConnectInfo.Create(ConnectStatus.LoggedInAgain);
            }
            if (IsInGame() && !SessionManager<PlayerSessionData>.Instance.IsReconnecting(payload.playerGuid))
            {
                return ConnectInfo.Create(ConnectStatus.UserDefined);
            }
            
            return ConnectInfo.Create(ConnectStatus.Success);
        }

        private bool IsInGame()
        {
            var state = Object.FindObjectOfType<GameStateBehaviour<GameState.GameState>>();
            return state.State is GameState.GameState.InGame;
        }
        
        private bool IsInLobby()
        {
            var state = Object.FindObjectOfType<GameStateBehaviour<GameState.GameState>>();
            return state.State is GameState.GameState.Lobby;
        }
    }
}