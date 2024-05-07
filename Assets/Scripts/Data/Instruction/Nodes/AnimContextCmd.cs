using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Animation;
using XNode;

namespace Data.Instruction.Nodes
{
    /// <summary>
    /// 执行播放动画命令。
    /// </summary>
    [NodeTint("#558800")]
    public class AnimContextCmd : CommandBase
    {
        public AnimTargetType sourceType;

        public AnimTargetType targetType;
        
        [Input] public List<ulong> targetList;
        
        [Output] public AnimContext animContext;

        public override async Task<bool> Execute(ICmdContext context, TempContext tmpContext)
        {
            targetList = GetInputValue<List<ulong>>(nameof(targetList));
            targetList ??= new List<ulong>();
            animContext = new AnimContext()
            {
                source = new AnimTarget
                {
                    id = tmpContext.SubjectID,
                    type = sourceType,
                },
                targets = targetList.Select(id => new AnimTarget() {id = id, type = targetType}).ToList()
            };
            await Task.CompletedTask;
            return true;
        }

        public override object GetValue(NodePort port)
        {
            return animContext;
        }
    }
}