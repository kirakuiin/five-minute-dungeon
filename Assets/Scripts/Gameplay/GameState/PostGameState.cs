using System.Collections.Generic;
using GameLib.Common.Behaviour;
using Unity.Netcode;
using UnityEngine.SceneManagement;

namespace Gameplay.GameState
{
    /// <summary>
    /// 游戏后结算状态。
    /// </summary>
    public class PostGameState : GameStateBehaviour<GameState>
    {
        public override GameState State => GameState.PostGame;
        
        private void Awake()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += OnLoadEventCompleted; 
            }
        }

        private void OnLoadEventCompleted(string sceneName, LoadSceneMode mode, List<ulong> clientID, List<ulong> timeouts)
        {
        }
    }
}