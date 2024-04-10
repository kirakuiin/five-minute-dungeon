using Common;
using GameLib.Network.NGO;
using GameLib.Network.NGO.ConnectionManagement;

namespace Gameplay.Connection
{
    public class ClientConnecting : ClientConnectingState
    {
        public ClientConnecting(ConnectionMethod method) : base(method)
        {
        }
    }
}