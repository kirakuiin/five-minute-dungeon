using System.Threading.Tasks;

namespace Data.Animation.Nodes
{
    /// <summary>
    /// 时停动画指令。
    /// </summary>
    public class DissolveResCmd : AnimationBase
    {
        public override async Task Execute(IBehaveController controller, AnimContext animContext)
        {
            await controller.GetVfxPlayer().PlayDissolveRes(animContext.other.selectedRes);
        }
    }
}