namespace Gameplay.Core
{
    /// <summary>
    /// 游戏玩法中的初始化各个阶段。
    /// </summary>
    public enum GamePlayInitStage
    {
        InitController,
        InitPile,
        ReconnectEvent,
    }

    public enum LocalSyncInitStage
    {
        InitPlayerController,
        InitPile,
    }
}