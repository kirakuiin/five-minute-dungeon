﻿using UnityEngine;

namespace Common
{
    /// <summary>
    /// 游戏设定。
    /// </summary>
    public static class GameConfig
    {
        public const int MaxPlayerNum = 5;

        public const int ScreenWidth = 1920;
        
        public const int ScreenHeight = 1080;
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

        /// <summary>
        /// ping最大延迟时间
        /// </summary>
        public static readonly int Timeout = 5;
    }

    /// <summary>
    /// 场景名称定义。
    /// </summary>
    public static class SceneDefines
    {
        /// <summary>
        /// 主界面
        /// </summary>
        public const string MainUI = "MainUI";

        /// <summary>
        /// 房间界面
        /// </summary>
        public const string LobbyUI = "LobbyUI";

        /// <summary>
        /// 游戏玩法界面
        /// </summary>
        public const string GamePlay = "GamePlay";

        /// <summary>
        /// 游戏结算界面
        /// </summary>
        public const string PostGame = "PostGame";
    }

    /// <summary>
    /// 游戏房间状态。
    /// </summary>
    public enum LobbyState
    {
        WaitForPlayer,
        InGame,
    }

    public static class TagDefines
    {
        private const string DiscardArea = "DiscardArea";

        private const string PlayArea = "PlayArea";

        /// <summary>
        /// 是否为弃牌区。
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static bool IsDiscardArea(string tag) => DiscardArea == tag;

        /// <summary>
        /// 是否为出牌区。
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static bool IsPlayArea(string tag) => PlayArea == tag;
    }
}