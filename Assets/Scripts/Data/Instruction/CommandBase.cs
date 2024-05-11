using System.Threading.Tasks;
using XNode;

namespace Data.Instruction
{
    /// <summary>
    /// 所有指令的基类。
    /// </summary>
    public abstract class CommandBase : Node
    {
        private InstructionGraph Graph => graph as InstructionGraph;

        /// <summary>
        /// 是否仅允许在服务端执行。
        /// </summary>
        public bool onlyExecuteOnServer = false;

        /// <summary>
        /// 执行指令。
        /// </summary>
        /// <param name="context">执行指令所需要的外部上下文。</param>
        /// <param name="tmpContext">执行指令所需要的临时上下文。</param>
        /// <returns></returns>
        public abstract Task<bool> Execute(ICmdContext context, TempContext tmpContext);

        protected InstructionSubject Subject => Graph.subject;
    }
}