using Common;
using GameLib.Network.NGO;
using GameLib.Network.NGO.ConnectionManagement;

namespace Gameplay.Connection
{
    public class ClientReconnecting : ClientReconnectingState
    {
        public ClientReconnecting(ConnectionMethod method) : base(method)
        {
        }

        public override void Exit()
        {
            base.Exit();
            SceneLoader.Instance.LoadScene(SceneDefines.MainUI);
        }
    }
}