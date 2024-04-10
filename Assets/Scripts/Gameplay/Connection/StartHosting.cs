using GameLib.Common;
using GameLib.Network.NGO;
using GameLib.Network.NGO.ConnectionManagement;
using Gameplay.Data;
using Unity.Netcode;
using UnityEngine;

namespace Gameplay.Connection
{
    public class StartHosting : StartHostingState
    {
        public StartHosting(ConnectionMethod method) : base(method)
        {
        }

        protected override void SetResponse(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
        {
            var payload = SerializeTool.Deserialize<Protocol.ConnectionPayload>(request.Payload);
            var clientID = request.ClientNetworkId;
            
            SessionManager<PlayerSessionData>.Instance.SetupPlayerData(clientID, payload.playerGuid,
                new PlayerSessionData(){IsConnected = true, PlayerID = payload.playerGuid, ClientID = clientID});
            Debug.Log($"保存玩家{payload.playerGuid}的会话数据。");
            
            response.Approved = true;
            response.CreatePlayerObject = false;
        }
    }
}