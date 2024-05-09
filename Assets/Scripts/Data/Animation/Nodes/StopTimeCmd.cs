using System.Threading.Tasks;

namespace Data.Animation.Nodes
{
    /// <summary>
    /// 时停动画指令。
    /// </summary>
    public class StopTimeCmd : AnimationBase
    {
        public override async Task Execute(IBehaveController controller, AnimContext animContext)
        {
            var pos = controller.GetPositionInfo().GetAnimTargetPos(animContext.source);
            await controller.GetVfxPlayer().TimeStop(pos);
        }
    }
}