using Data.Instruction;

namespace Data.Check.Nodes
{
    /// <summary>
    /// 代表一直为真。
    /// </summary>
    public class AlwaysTrueCmd : CheckBase
    {
        public override bool Execute(IRuntimeContext context, TempContext tmpContext)
        {
            return true;
        }
    }
}