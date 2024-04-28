using System.Linq;
using Data.Instruction;

namespace Data.Check.Nodes
{
    /// <summary>
    /// 检查是否需要资源。
    /// </summary>
    public class NeedResCmd: CheckBase
    {
        public override bool Execute(IRuntimeContext context, TempContext tmpContext)
        {
            return context.GetLevelRuntimeInfo().GetCurNeedResources().Any();
        }
    }
}