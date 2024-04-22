using System;
using System.Collections.Generic;

namespace Data.Check
{
    public interface IInteractiveRuntime
    {
        /// <summary>
        /// 获得敌方选择器。
        /// </summary>
        /// <returns></returns>
        public IEnemySelector GetEnemySelector();

        /// <summary>
        /// 获得资源选择器。
        /// </summary>
        /// <returns></returns>
        public IResourceSelector GetResourceSelector();

        /// <summary>
        /// 获得手牌选择器。
        /// </summary>
        /// <returns></returns>
        public IHandSelector GetHandSelector();

        /// <summary>
        /// 获得玩家选择器。
        /// </summary>
        /// <returns></returns>
        public IPlayerSelector GetPlayerSelector();
    }
    
    /// <summary>
    /// 敌人选择器。
    /// </summary>
    public interface IEnemySelector
    {
        /// <summary>
        /// 开始选择敌人时触发。
        /// </summary>
        public event Action<EnemyCardType> OnEnemySelecting;

        /// <summary>
        /// 选择敌方单位。
        /// </summary>
        /// <param name="enemyID"></param>
        public void SelectEnemy(ulong enemyID);
    }

    /// <summary>
    /// 玩家选择器。
    /// </summary>
    public interface IPlayerSelector
    {
        /// <summary>
        /// 开始选择玩家时触发。
        /// </summary>
        public event Action<int, bool> OnPlayerSelecting;

        /// <summary>
        /// 选择玩家。
        /// </summary>
        /// <param name="clientIDs"></param>
        public void SelectPlayer(IEnumerable<ulong> clientIDs);
    }

    /// <summary>
    /// 资源选择器。
    /// </summary>
    public interface IResourceSelector
    {
        /// <summary>
        /// 开始选择资源时触发。
        /// </summary>
        public event Action OnResourceSelecting;

        /// <summary>
        /// 选择一种资源类型。
        /// </summary>
        /// <param name="type"></param>
        public void SelectResource(Resource type);
    }

    /// <summary>
    /// 手牌选择器。
    /// </summary>
    public interface IHandSelector
    {
        /// <summary>
        /// 开始选择手牌时触发。
        /// </summary>
        public event Action<int> OnHandSelecting;

        public void SelectHand(IEnumerable<Card> cards);
    }
}