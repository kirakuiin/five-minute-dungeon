using System.Collections.Generic;
using System.Linq;
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
        [Input] public List<ulong> targetList;

        [Input(dynamicPortList = true)] public List<ulong> dynamicTargetList;
        
        public AnimTargetType sourceType;

        public AnimTargetType targetType;

        public bool isDynamicTarget;
        
        public AnimGraph animGraph;

        public bool haveOtherInfo;

        [Input] public Resource selectRes;
        
        public override async Task<bool> Execute(ICmdContext context, TempContext tmpContext)
        {
            var finalTargetList = isDynamicTarget
                ? GetDynamicValue()
                : GetInputValue<List<ulong>>(nameof(targetList));
            finalTargetList ??= new List<ulong>();
            
            var animContext = new AnimContext()
            {
                source = new AnimTarget
                {
                    id = tmpContext.SubjectID,
                    type = sourceType,
                },
                targets = finalTargetList.Select(id => new AnimTarget() {id = id, type = targetType}).ToList()
            };
            if (haveOtherInfo)
            {
                animContext.other = new OtherAnimInfo() { selectedRes = GetInputValue<Resource>(nameof(selectRes)) };
            }
            
            await animGraph.Execution(context.GetBehaveController(), animContext);
            return true;
        }

        private List<ulong> GetDynamicValue()
        {
            return Enumerable.Range(0, dynamicTargetList.Count).Select(
                idx => GetInputValue<ulong>($"{nameof(dynamicTargetList)} {idx}")).ToList();
        }
    }
}