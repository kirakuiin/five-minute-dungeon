using UnityEngine;
using GameLib.Common;

namespace Audio
{
    /// <summary>
    /// 背景音乐播放器。
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
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

        private AudioSource _audioSource;

        protected override void OnInitialized()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        /// <summary>
        /// 播放主界面音乐。
        /// </summary>
        public void PlayMainUI()
        {
            Play(mainUIClip);
        }

        /// <summary>
        /// 播放普通战斗背景音乐。
        /// </summary>
        public void PlayBattle()
        {
            Play(battleClip);
        }

        /// <summary>
        /// 播放boss背景音乐。
        /// </summary>
        public void PlayPost(bool isWin)
        {
            Play(isWin ? postWinClip : postLostClip);
        }

        private void Play(AudioClip clip)
        {
            if (_audioSource.isPlaying)
            {
                if (_audioSource.clip == clip) return;
                _audioSource.Stop();
                _audioSource.time = 0;
            }
            _audioSource.clip = clip;
            _audioSource.loop = true;
            _audioSource.Play();
        }

        /// <summary>
        /// 停止播放音乐。
        /// </summary>
        public void Pause()
        {
            _audioSource.Pause();
        }
    }
}