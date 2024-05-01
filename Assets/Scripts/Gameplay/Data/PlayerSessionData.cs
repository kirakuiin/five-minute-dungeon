using Data;
using GameLib.Network.NGO;

namespace Gameplay.Data
{
    /// <summary>
    /// 管理玩家会话数据。
    /// </summary>
    public struct PlayerSessionData : ISessionPlayerData
    {
        public bool IsConnected { set; get; }
        
        public ulong ClientID { set; get; }
        
        public string PlayerID { set; get; }
        
        /// <summary>
        /// 玩家名称。
        /// </summary>
        public string PlayerName { set; get; }
        
        /// <summary>
        /// 玩家选择职业。
        /// </summary>
        public Class PlayerClass { set; get; }
        
        /// <summary>
        /// 是否已经游玩过一局？
        /// </summary>
        public bool HaveBeenPlayed { set; get; }
        
        public void Reinitialize()
        {
            HaveBeenPlayed = true;
        }
    }
}