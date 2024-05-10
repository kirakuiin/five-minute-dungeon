using System.Threading.Tasks;
using UnityEngine;

namespace Data.Animation.Nodes
{
    /// <summary>
    /// 移动玩家指令。
    /// </summary>
    public class MovePlayerCmd : AnimationBase
    {
        public Vector3 offset;
        
        public override async Task Execute(IBehaveController controller, AnimContext animContext)
        {
            if (animContext.targets.Count == 1)
            {
                var pos = controller.GetPositionInfo().GetAnimTargetPos(animContext.targets[0]);
                controller.GetModelPlayer(animContext.source).MoveTo(pos+offset);
            }

            await Task.CompletedTask;
        }
    }
}