namespace Common
{
    /// <summary>
    /// 游戏设定。
    /// </summary>
    public static class GameConfig
    {
        public const int MaxPlayerNum = 5;
    }
    
    /// <summary>
    /// 音量关键字定义
    /// </summary>
    public static class VolumeKey
    {
        public static readonly string Master = "MasterVolume";
        public static readonly string Music = "MusicVolume";
        public static readonly string Effect = "EffectVolume";
    }

    /// <summary>
    /// 网络相关常量定义。
    /// </summary>
    public static class NetworkDefines
    {
        /// <summary>
        /// 游戏端口号。
        /// </summary>
        public static readonly ushort Port = 23132;

        public static readonly int Timeout = 5;

        public static readonly int InteractiveTimeout = 60;
    }

    /// <summary>
    /// 场景名称定义。
    /// </summary>
    public static class SceneDefines
    {
        /// <summary>
        /// 主界面
        /// </summary>
        public static readonly string MainUI = "MainUI";
        
        /// <summary>
        /// 房间界面
        /// </summary>
        public static readonly string LobbyUI = "LobbyUI";
        
        
        /// <summary>
        /// 游戏玩法界面
        /// </summary>
        public static readonly string GamePlay = "GamePlay";
    }

    /// <summary>
    /// 游戏房间状态。
    /// </summary>
    public enum LobbyState
    {
        WaitForPlayer,
        InGame,
    }
}