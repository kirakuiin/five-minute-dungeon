using Common;
using GameLib.Common;
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
            var status = GetConnectStatus(request);
            if (status == ConnectStatus.Success)
            {
                response.Approved = true;
                response.CreatePlayerObject = false;
                RegisterPlayerSessionData(request);
            }
            else
            {
                response.Approved = false;
                response.Reason = JsonUtility.ToJson(status);
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

        protected override ConnectStatus GetConnectStatus(NetworkManager.ConnectionApprovalRequest request)
        {
            var payload = SerializeTool.Deserialize<Protocol.ConnectionPayload>(request.Payload);
            if (NetManager.ConnectedClientsIds.Count >= ConnManager.config.maxConnectedPlayerNum)
            {
                return ConnectStatus.ServerFull;
            }
            if (payload.password != PlayerSetting.Instance.LobbyPassword)
            {
                return ConnectStatus.ApprovalFailed;
            }
            if (SessionManager<PlayerSessionData>.Instance.IsDuplicateConnection(payload.playerGuid))
            {
                return ConnectStatus.LoggedInAgain;
            }
            
            return ConnectStatus.Success;
        }
    }
}