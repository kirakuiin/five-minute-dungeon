using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Animation;

namespace Data.Instruction.Nodes
{
    /// <summary>
    /// 执行播放动画命令。
    /// </summary>
    public class PlayAnimCmd : CommandBase
    {
        [Input] public AnimTarget source;

        [Input] public List<AnimTarget> targetList;
        
        public override async Task Execute(ICmdContext context, TempContext tmpContext)
        {
            await Task.CompletedTask;
        }
    }
}