using Common;
using GameLib.Network.NGO;
using GameLib.Network.NGO.ConnectionManagement;

namespace Gameplay.Connection
{
    public class ClientConnected : ClientConnectedState
    {
        public override void OnUserRequestShutdown()
        {
            base.OnUserRequestShutdown();
            SceneLoader.Instance.LoadScene(SceneDefines.MainUI);
        }
    }
}