using System;
using UnityEngine;

namespace Gameplay.GameState
{
    /// <summary>
    /// 游戏当前所处状态。
    /// </summary>
    public enum GameState
    {
        Startup,
        MainMenu,
        Lobby,
        InGame,
        PostGame,
    }
}