using Common;
using GameLib.Common;
using UnityEngine;

namespace Save
{
    /// <summary>
    /// 本地存储玩家设置。
    /// </summary>
    public class PlayerSetting : Singleton<PlayerSetting>
    {
        private const float DefaultVolume = 0.5f;

        private const string DefaultLobbyName = "lobby";

        private const string DefaultPassword = "";

        private const string DefaultPlayerName = "叮";

        private const string PlayerNameKey = "PlayerName";

        private const string LobbyNameKey = "LobbyName";

        private const string LobbyPasswordKey = "LobbyPassword";

        private readonly string _prefix = System.Environment.CurrentDirectory;
        
        /// <summary>
        /// 控制主音量
        /// </summary>
        public float MasterVolume
        {
            set => PlayerPrefs.SetFloat(GetKey(VolumeKey.Master), value);
            get => PlayerPrefs.GetFloat(GetKey(VolumeKey.Master), DefaultVolume);
        }

        private string GetKey(string key)
        {
#if UNITY_EDITOR
            return $"{_prefix}{key}";
#else
            return key;
#endif
        }
        
        /// <summary>
        /// 控制音乐音量
        /// </summary>
        public float MusicVolume
        {
            set => PlayerPrefs.SetFloat(GetKey(VolumeKey.Music), value);
            get => PlayerPrefs.GetFloat(GetKey(VolumeKey.Music), DefaultVolume);
        }
        
        /// <summary>
        /// 控制音效音量
        /// </summary>
        public float EffectVolume
        {
            set => PlayerPrefs.SetFloat(GetKey(VolumeKey.Effect), value);
            get => PlayerPrefs.GetFloat(GetKey(VolumeKey.Effect), DefaultVolume);
        }

        /// <summary>
        /// 房间名称。
        /// </summary>
        public string LobbyName
        {
            set => PlayerPrefs.SetString(GetKey(LobbyNameKey), value);
            get => PlayerPrefs.GetString(GetKey(LobbyNameKey), DefaultLobbyName);
        }

        /// <summary>
        /// 玩家名称。
        /// </summary>
        public string PlayerName
        {
            set => PlayerPrefs.SetString(GetKey(PlayerNameKey), value);
            get => PlayerPrefs.GetString(GetKey(PlayerNameKey), DefaultPlayerName);
        }

        /// <summary>
        /// 房间密码。
        /// </summary>
        public string LobbyPassword
        {
            set => PlayerPrefs.SetString(GetKey(LobbyPasswordKey), value);
            get => PlayerPrefs.GetString(GetKey(LobbyPasswordKey), DefaultPassword);
        }
    }
}