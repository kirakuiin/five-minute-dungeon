using UnityEngine;

namespace Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class VfxAudio : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<AudioSource>().outputAudioMixerGroup = VolumeConfigurator.Instance.GetEffectGroup();
        }
    }
}