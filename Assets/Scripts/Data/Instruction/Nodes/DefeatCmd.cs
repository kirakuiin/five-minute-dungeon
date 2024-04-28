using System.Threading.Tasks;
using Common;

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
        
        public override async Task Execute(ICmdContext context, TempContext tempContext)
        {
            enemyID = GetInputValue<ulong>(nameof(enemyID));
            var controller = context.GetLevelController();
            if (enemyID != EnemyIDDefine.Invalid)
            {
                controller.DestroyEnemyCard(enemyID);
            }
            await Task.CompletedTask;
        }
    }
}