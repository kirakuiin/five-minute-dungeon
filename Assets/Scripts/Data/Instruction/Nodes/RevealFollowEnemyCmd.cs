using System.Threading.Tasks;

namespace Data.Instruction.Nodes
{
    /// <summary>
    /// 揭露接下来的敌人。
    /// </summary>
    public class RevealFollowEnemyCmd : CommandBase
    {
        /// <summary>
        /// 揭露数量。
        /// </summary>
        public int num;

        public override async Task<bool> Execute(ICmdContext context, TempContext tempContext)
        {
            var levelController = context.GetLevelController();
            if (levelController.IsReachBoss()) return false;
            levelController.RevealNextLevel(num);
            await Task.CompletedTask;
            return true;
        }
    }
}