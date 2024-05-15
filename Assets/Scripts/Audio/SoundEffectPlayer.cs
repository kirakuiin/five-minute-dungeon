using Data;
using Data.Animation;
using UnityEngine;

namespace Audio
{
    /// <summary>
    /// 音效播放器。
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class SoundEffectPlayer : MonoBehaviour
    {
        private AudioSource _audio;

        private void Awake()
        {
            _audio = GetComponent<AudioSource>();
        }

        /// <summary>
        /// 播放音效。
        /// </summary>
        /// <param name="param"></param>
        public void Play(AudioParam param)
        {
            var info = DataService.Instance.GetAudioData(param.clipName);
            _audio.PlayOneShot(info.GetRandom(), param.volume);
        }
    }
}