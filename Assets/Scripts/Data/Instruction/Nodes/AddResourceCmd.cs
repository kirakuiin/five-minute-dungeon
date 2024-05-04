using System.Threading.Tasks;

namespace Data.Instruction.Nodes
{
    /// <summary>
    /// 添加资源。
    /// </summary>
    public class AddResourceCmd : CommandBase
    {
        /// <summary>
        /// 资源类型。
        /// </summary>
        public Resource resource;

        /// <summary>
        /// 资源数量。
        /// </summary>
        public int num;

        public override async Task<bool> Execute(ICmdContext context, TempContext tempContext)
        {
            var controller = context.GetLevelController();
            controller.AddResource(resource, num);
            await Task.CompletedTask;
            return true;
        }
    }
}