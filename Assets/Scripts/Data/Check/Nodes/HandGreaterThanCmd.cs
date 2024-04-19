using Data.Instruction;

namespace Data.Check.Nodes
{
    /// <summary>
    /// 手牌数量检查。
    /// </summary>
    public class HandGreaterThanCmd : CheckBase
    {
        public int handSize;

        public override bool Execute(IRuntimeContext context, TempContext tmpContext)
        {
            return context.GetPlayerRuntimeInfo(tmpContext.ClientID).GetHands().Count > handSize;
        }
    }
}