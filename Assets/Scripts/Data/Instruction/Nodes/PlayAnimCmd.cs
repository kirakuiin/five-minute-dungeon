using System.Threading.Tasks;
using Data.Animation;

namespace Data.Instruction.Nodes
{
    /// <summary>
    /// 执行播放动画命令。
    /// </summary>
    [NodeTint("#558800")]
    public class PlayAnimCmd : CommandBase
    {
        [Input] public AnimContext animContext;

        public AnimGraph animGraph;
        
        public override async Task<bool> Execute(ICmdContext context, TempContext tmpContext)
        {
            animContext = GetInputValue<AnimContext>(nameof(animContext));
            await animGraph.Execution(context.GetBehaveController(), animContext);
            return true;
        }
    }
}