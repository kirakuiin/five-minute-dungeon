using GameLib.Audio;
using UnityEngine;
using GameLib.Common;

namespace Audio
{
    /// <summary>
    /// 背景音乐播放器。
    /// </summary>
    [RequireComponent(typeof(MusicPlayer))]
    public class BgMusicPlayer : PersistentMonoSingleton<BgMusicPlayer>
    {
        [Tooltip("主界面背景音乐")]
        [SerializeField]
        private AudioClip mainUIClip;
        
        [Tooltip("普通战斗背景音乐")]
        [SerializeField]
        private AudioClip battleClip;
        
        [Tooltip("胜利背景音乐")]
        [SerializeField]
        private AudioClip postWinClip;
        
        [Tooltip("失败背景音乐")]
        [SerializeField]
        private AudioClip postLostClip;

        [Tooltip("音乐播放器")]
        [SerializeField]
        private MusicPlayer player;

        /// <summary>
        /// 播放主界面音乐。
        /// </summary>
        public void PlayMainUI()
        {
            player.PlayTrack(mainUIClip, true);
        }

        /// <summary>
        /// 播放普通战斗背景音乐。
        /// </summary>
        public void PlayBattle()
        {
            player.PlayTrack(battleClip, true);
        }

        /// <summary>
        /// 播放boss背景音乐。
        /// </summary>
        public void PlayPost(bool isWin)
        {
            player.PlayTrack(isWin ? postWinClip : postLostClip, true);
        }
    }
}