using Data;
using Gameplay.Data;

namespace Gameplay.Core
{
    /// <summary>
    /// 获得玩家运行时数据。
    /// </summary>
    public interface IPlayerRuntimeInfo
    {
        /// <summary>
        /// 玩家职业。
        /// </summary>
        public Class PlayerClass { get; }
        
        /// <summary>
        /// 玩家手牌。
        /// </summary>
        /// <returns></returns>
        public ICardCollectionsInfo GetHands();

        /// <summary>
        /// 玩家牌库。
        /// </summary>
        /// <returns></returns>
        public ICardCollectionsInfo GetDraws();

        /// <summary>
        /// 玩家弃牌区。
        /// </summary>
        /// <returns></returns>
        public ICardCollectionsInfo GetDiscards();
    }
}