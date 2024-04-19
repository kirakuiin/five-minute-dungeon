using System.Linq;
using Data.Instruction;

namespace Data.Check.Nodes
{
    /// <summary>
    /// 检查是否存在指定类型的怪物。
    /// </summary>
    public class ExistEnemyCmd : CheckBase
    {
        public EnemyCardType enemyType;
        
        public override bool Execute(IRuntimeContext context, TempContext tmpContext)
        {
            return context.GetLevelRuntimeInfo().GetAllEnemiesInfo().Values.Any(enemy => (enemy.type & enemyType) > 0);
        }
    }
}