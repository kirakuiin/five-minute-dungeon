using Common;
using GameLib.Common.Behaviour;
using GameLib.Network.NGO;
using GameLib.Network.NGO.ConnectionManagement;

namespace Gameplay.GameState
{
    /// <summary>
    /// 游戏后结算状态。
    /// </summary>
    public class PostGameState : GameStateBehaviour<GameState>
    {
        public override GameState State => GameState.PostGame;
        
        public void GoBackToMain()
        {
            ConnectionManager.Instance.UserRequestShutdown();
        }

        public void GoBackToLobby()
        {
            SceneLoader.Instance.LoadSceneByNet(SceneDefines.LobbyUI);
        }

        public void GoBackToGamePlay()
        {
            SceneLoader.Instance.LoadSceneByNet(SceneDefines.GamePlay);
        }
    }
}