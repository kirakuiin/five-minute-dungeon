using Audio;
using Popup;
using Save;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Menu
{
    public class GameSettingController : CacheablePopupBehaviour
    {
        [SerializeField]
        private Slider masterSlider;
        
        [SerializeField]
        private Slider musicSlider;
        
        [SerializeField]
        private Slider effectSlider;

        [SerializeField]
        private Toggle fullScreenToggle;

        private void Awake()
        {
            LoadSetting();
        }

        private void LoadSetting()
        {
            masterSlider.value = PlayerSetting.Instance.MasterVolume;
            musicSlider.value = PlayerSetting.Instance.MusicVolume;
            effectSlider.value = PlayerSetting.Instance.EffectVolume;
            fullScreenToggle.isOn = PlayerSetting.Instance.IsFullScreen;
        }

        public void OnMasterValueChanged(float value)
        {
            VolumeConfigurator.Instance.SetMasterVolume(value);
            PlayerSetting.Instance.MasterVolume = value;
        }
        
        public void OnMusicValueChanged(float value)
        {
            VolumeConfigurator.Instance.SetMusicVolume(value);
            PlayerSetting.Instance.MusicVolume = value;
        }
        
        public void OnEffectValueChanged(float value)
        {
            VolumeConfigurator.Instance.SetEffectVolume(value);
            PlayerSetting.Instance.EffectVolume = value;
        }

        public void OnFullScreen(bool isOn)
        {
            Screen.fullScreen = isOn;
            PlayerSetting.Instance.IsFullScreen = isOn;
        }

        public void OnPointerExit(BaseEventData eventData)
        {
            gameObject.SetActive(false);
        }
    }
}