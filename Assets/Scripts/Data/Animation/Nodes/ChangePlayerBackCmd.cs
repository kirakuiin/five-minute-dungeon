using System.Threading.Tasks;

namespace Data.Animation.Nodes
{
    /// <summary>
    /// 将玩家还原初始状态。
    /// </summary>
    public class ChangePlayerBackCmd : AnimationBase
    {
        public override async Task Execute(IBehaveController controller, AnimContext animContext)
        {
            controller.GetModelPlayer(animContext.source).ChangeBack();
            await Task.CompletedTask;
        }
    }
}