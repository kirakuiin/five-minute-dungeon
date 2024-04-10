namespace Data.Instruction
{
    /// <summary>
    /// 玩家目标。
    /// </summary>
    public enum PlayerTarget
    {
        /// <summary>
        /// 自身。
        /// </summary>
        Self,
        
        /// <summary>
        /// 某个具体玩家。
        /// </summary>
        SpecificPlayer,
        
        /// <summary>
        /// 另一位玩家。
        /// </summary>
        AnotherPlayer,
        
        /// <summary>
        /// 所有玩家。
        /// </summary>
        AllPlayer,
        
        /// <summary>
        /// 除了自己以外的全部玩家。
        /// </summary>
        AllExceptSelf,
    }

    /// <summary>
    /// 发起指令的主体。
    /// </summary>
    public enum InstructionSubject
    {
        /// <summary>
        /// 出牌玩家。
        /// </summary>
        ThePlayerWhoPlay,
        
        /// <summary>
        /// 地城系统。
        /// </summary>
        Dungeon,
        
        /// <summary>
        /// 全部玩家。
        /// </summary>
        AllPlayer,
    }
}