using System.Linq;
using Common;
using GameLib.Audio;
using GameLib.Common;
using Save;
using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
    /// <summary>
    /// 音量混合控制器
    /// </summary>
    [RequireComponent(typeof(AudioMixerConfigurator))]
    public class VolumeConfigurator : PersistentMonoSingleton<VolumeConfigurator>
    {
        [Tooltip("混音配置器")]
        [SerializeField]
        private AudioMixerConfigurator configurator;

        [SerializeField]
        private AudioMixer mixer;

        private void Start()
        {
            SetMasterVolume(PlayerSetting.Instance.MasterVolume);
            SetMusicVolume(PlayerSetting.Instance.MusicVolume);
            SetEffectVolume(PlayerSetting.Instance.EffectVolume);
        }

        /// <summary>
        /// 设置主音量，会影响音效和背景音量。
        /// </summary>
        /// <param name="volume">声音大小(0-1)</param>
        public void SetMasterVolume(float volume)
        {
            configurator.SetFloat(VolumeKey.Master, volume);
        }
        
        /// <summary>
        /// 设置音乐音量。
        /// </summary>
        /// <param name="volume">声音大小(0-1)</param>
        public void SetMusicVolume(float volume)
        {
            configurator.SetFloat(VolumeKey.Music, volume);
        }
        
        /// <summary>
        /// 设置音效音量。
        /// </summary>
        /// <param name="volume">声音大小(0-1)</param>
        public void SetEffectVolume(float volume)
        {
            configurator.SetFloat(VolumeKey.Effect, volume);
        }

        /// <summary>
        /// 获得音效组。
        /// </summary>
        /// <returns></returns>
        public AudioMixerGroup GetEffectGroup()
        {
            return mixer.FindMatchingGroups("Master/Effect").First();
        }
    }
}