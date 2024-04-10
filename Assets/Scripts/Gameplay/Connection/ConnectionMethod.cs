using System.Net;
using System.Threading.Tasks;
using GameLib.Common;
using GameLib.Network.NGO.ConnectionManagement;
using Unity.Netcode;
using UnityEngine.Device;

namespace Gameplay.Connection
{
    /// <summary>
    /// 使用IP地址和密码进行验证的连接方式。
    /// </summary>
    public class IPPassConnectionMethod : DirectIPConnectionMethod
    {
        private readonly string _password;
        
        public IPPassConnectionMethod(IPEndPoint endPoint, string password) : base(endPoint)
        {
            _password = password;
            // TODO() : 测试完毕后还原
            PlayerID = System.Environment.CurrentDirectory;
        }

        protected override void SetConnectionPayload()
        {
            var payload = new Protocol.ConnectionPayload()
            {
                playerGuid = PlayerID,
                password = _password
            };
            NetworkManager.Singleton.NetworkConfig.ConnectionData = SerializeTool.Serialize(payload);
        }
    }
}