using Unity.Netcode;

namespace Gameplay.Message
{
    /// <summary>
    /// 游戏中状态。
    /// </summary>
    public struct GamePlayStateMsg : INetworkSerializeByMemcpy
    {
        public GamePlayStateEnum state;
        
        /// <summary>
        /// 工厂函数。
        /// </summary>
        /// <param name="stat"></param>
        /// <returns></returns>
        public static GamePlayStateMsg Create(GamePlayStateEnum stat)
        {
            return new GamePlayStateMsg() { state = stat };
        }

        public override string ToString()
        {
            return $"{state}";
        }
    }

    public enum GamePlayStateEnum
    {
        NotStart,
        InitDone,
        Running,
        End,
    }
    
}