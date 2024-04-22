using System;
using System.Collections.Generic;

namespace Data.Check
{
    /// <summary>
    /// 运行时上下文环境。
    /// </summary>
    public interface IRuntimeContext
    {
        /// <summary>
        /// 获得玩家运行时信息。
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        public IPlayerRuntimeInfo GetPlayerRuntimeInfo(ulong clientID);
        
        /// <summary>
        /// 获得本地玩家运行时信息。
        /// </summary>
        /// <returns></returns>
        public IPlayerRuntimeInfo GetPlayerRuntimeInfo();

        /// <summary>
        /// 获得关卡运行时信息。
        /// </summary>
        /// <returns></returns>
        public ILevelRuntimeInfo GetLevelRuntimeInfo();

        /// <summary>
        /// 获得时间的运行时信息。
        /// </summary>
        /// <returns></returns>
        public ITimeRuntimeInfo GetTimeRuntimeInfo();
    }
    
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
        
        /// <summary>
        /// 获得运行时交互器。
        /// </summary>
        /// <returns></returns>
        public IRuntimeInteractive GetRuntimeInteractive();
    }

    /// <summary>
    /// 敌人信息变化事件。
    /// </summary>
    public struct EnemyChangeEvent
    {
        public ulong enemyID;
        public EnemyCard enemyCard;
    }

    /// <summary>
    /// 获得关卡运行时数据。
    /// </summary>
    public interface ILevelRuntimeInfo
    {
        /// <summary>
        /// 添加新的敌人时触发。
        /// </summary>
        public event Action<EnemyChangeEvent> OnEnemyAdded;

        /// <summary>
        /// 消灭了敌人时触发。
        /// </summary>
        public event Action<EnemyChangeEvent> OnEnemyDestroyed;

        /// <summary>
        /// 有玩家投入新的资源时触发。
        /// </summary>
        public event Action<Resource, int> OnResourceAdded;

        /// <summary>
        /// 获得当前关卡需要的资源。
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Resource> GetCurNeedResources();

        /// <summary>
        /// 获得已经投入的资源。
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Resource> GetAlreadyPlayedResources();

        /// <summary>
        /// 获得当前场上的敌人信息。
        /// </summary>
        /// <returns></returns>
        public IReadOnlyDictionary<ulong, EnemyCard> GetAllEnemiesInfo();
        
        /// <summary>
        /// 当前进度。
        /// </summary>
        public int CurProgress { get; }
        
        /// <summary>
        /// 总关卡数目。
        /// </summary>
        public int TotalLevelNum { get; }
        
        /// <summary>
        /// 是否包含事件。
        /// </summary>
        /// <returns></returns>
        public bool IsContainEvent()
        {
            foreach (var enemyCard in GetAllEnemiesInfo().Values)
            {
                if (enemyCard.IsEventCard())
                {
                    return true;
                }
            }

            return false;
        }
    }

    public interface ITimeRuntimeInfo
    {
        /// <summary>
        /// 剩余时间。
        /// </summary>
        public int RemainTime { get; }

        /// <summary>
        /// 时间更新时触发。
        /// </summary>
        public event Action<int> OnTimeUpdated;

        /// <summary>
        /// 计时器结束时触发。
        /// </summary>
        public event Action OnTimeout;
    }

    public interface IRuntimeInteractive
    {
        /// <summary>
        /// 获得敌方选择器。
        /// </summary>
        /// <returns></returns>
        public IEnemySelector GetEnemySelector(EnemyCardType type);

        /// <summary>
        /// 获得资源选择器。
        /// </summary>
        /// <returns></returns>
        public IResourceSelector GetResourceSelector();

        /// <summary>
        /// 获得玩家选择器。
        /// </summary>
        /// <returns></returns>
        public IPlayerSelector GetPlayerSelector();

        /// <summary>
        /// 获得手牌选择器。
        /// </summary>
        /// <returns></returns>
        public IHandSelector GetHandSelector();
    }
}