using System.Threading.Tasks;

namespace Data.Animation.Nodes
{
    /// <summary>
    /// 溶解角色指令。
    /// </summary>
    public class DissolveClassCmd : AnimationBase
    {
        public Class playerClass;
        
        public override async Task Execute(IBehaveController controller, AnimContext animContext)
        {
            await controller.GetVfxPlayer().PlayDissolveClass(playerClass);
        }
    }
}