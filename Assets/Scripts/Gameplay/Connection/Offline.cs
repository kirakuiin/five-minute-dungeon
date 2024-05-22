using Common;
using GameLib.Network.NGO;
using GameLib.Network.NGO.ConnectionManagement;
using UnityEngine.SceneManagement;

namespace Gameplay.Connection
{
    public class Offline : OfflineState
    {
        public override void Enter()
        {
            if (SceneManager.GetActiveScene().name != SceneDefines.MainUI)
            {
                SceneLoader.Instance.LoadScene(SceneDefines.MainUI);
            }
            base.Enter();
        }
    }
}