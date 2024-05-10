using System.Threading.Tasks;

namespace Data.Animation.Nodes
{
    /// <summary>
    /// 将玩家移动回原位。
    /// </summary>
    public class MovePlayerBackCmd : AnimationBase
    {
        public override async Task Execute(IBehaveController controller, AnimContext animContext)
        {
            controller.GetModelPlayer(animContext.source).MoveBack();
            await Task.CompletedTask;
        }
    }
}