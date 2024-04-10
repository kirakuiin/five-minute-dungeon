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
        
        /// <summary>
        /// 被击败的敌人的类型。
        /// </summary>
        public EnemyCardType enemyType;
        
        public override async Task Execute(ICmdContext context, TempContext tempContext)
        {
            enemyID = GetInputValue<ulong>(nameof(enemyID));
            var controller = context.GetLevelController();
            controller.DestroyEnemyCard(enemyID);
            await Task.CompletedTask;
        }
    }
}