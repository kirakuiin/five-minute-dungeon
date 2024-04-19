using System.Threading.Tasks;
using Data.Instruction;
using XNode;

namespace Data.Check
{
    /// <summary>
    /// 可行性检查节点基类。
    /// </summary>
    public abstract class CheckBase : Node
    {
        /// <summary>
        /// 执行指令。
        /// </summary>
        /// <param name="context">执行指令所需要的外部上下文。</param>
        /// <param name="tmpContext">执行指令所需要的临时上下文。</param>
        /// <returns></returns>
        public abstract bool Execute(IRuntimeContext context, TempContext tmpContext);
    }
}