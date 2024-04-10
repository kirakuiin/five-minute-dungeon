using Data.Instruction;

namespace Editor.Instruction
{
    /// <summary>
    /// 辅助函数。
    /// </summary>
    public static class InstructionHelper
    {
        /// <summary>
        /// 是否为单人目标。
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsSinglePlayer(PlayerTarget target)
        {
            return target != PlayerTarget.AllPlayer && target != PlayerTarget.AllExceptSelf;
        }
        
        /// <summary>
        /// 是否为非自身的单人玩家。
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsNotSelfSinglePlayer(PlayerTarget target)
        {
            return IsSinglePlayer(target) && target != PlayerTarget.Self;
        }
    }
}