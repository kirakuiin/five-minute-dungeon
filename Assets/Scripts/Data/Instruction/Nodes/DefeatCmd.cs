using System.Threading.Tasks;

namespace Data.Instruction.Nodes
{
    /// <summary>
    /// 击败某种类型的敌人。
    /// </summary>
    public class DefeatCmd : CommandBase
    {
        /// <summary>
        /// 敌方对象ID。
        /// </summary>
        [Input] public ulong enemyID;
        
        public override async Task<bool> Execute(ICmdContext context, TempContext tempContext)
        {
            enemyID = GetInputValue<ulong>(nameof(enemyID));
            var controller = context.GetLevelController();
            controller.DestroyEnemyCard(enemyID);
            await Task.CompletedTask;
            return true;
        }
    }
}