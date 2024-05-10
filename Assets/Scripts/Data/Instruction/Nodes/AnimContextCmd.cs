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

        public bool isDynamicTarget;
        
        [Input] public List<ulong> targetList;

        [Input(dynamicPortList = true)] public List<ulong> dynamicTargetList;
        
        [Output] public AnimContext animContext;

        public override async Task<bool> Execute(ICmdContext context, TempContext tmpContext)
        {
            var finalTargetList = isDynamicTarget
                ? GetDynamicValue()
                : GetInputValue<List<ulong>>(nameof(targetList));
            finalTargetList ??= new List<ulong>();
            
            animContext = new AnimContext()
            {
                source = new AnimTarget
                {
                    id = tmpContext.SubjectID,
                    type = sourceType,
                },
                targets = finalTargetList.Select(id => new AnimTarget() {id = id, type = targetType}).ToList()
            };
            await Task.CompletedTask;
            return true;
        }

        private List<ulong> GetDynamicValue()
        {
            return Enumerable.Range(0, dynamicTargetList.Count).Select(
                idx => GetInputValue<ulong>($"{nameof(dynamicTargetList)} {idx}")).ToList();
        }

        public override object GetValue(NodePort port)
        {
            return animContext;
        }
    }
}