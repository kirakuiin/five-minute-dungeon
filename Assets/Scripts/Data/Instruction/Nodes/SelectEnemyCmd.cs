using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XNode;

namespace Data.Instruction.Nodes
{
    /// <summary>
    /// 玩家选择一个敌方单位。
    /// </summary>
    public class SelectEnemyCmd : CommandBase
    {
        /// <summary>
        /// 敌方对象ID。
        /// </summary>
        [Output] public ulong enemyID;

        public EnemyCardType enemyType;

        public override object GetValue(NodePort port)
        {
            return enemyID;
        }

        public override async Task<bool> Execute(ICmdContext context, TempContext tempContext)
        {
            var enemies = context.GetLevelController().GetAllEnemiesInfo();
            var candidates = (from id in enemies.Keys
                where (enemyType & enemies[id].type) > 0
                select id).ToList();
            var result = await SetEnemyID(context, tempContext, candidates);
            return result;
        }

        private async Task<bool> SetEnemyID(ICmdContext context, TempContext tempContext, List<ulong> candidates)
        {
            var player = context.GetPlayerController(tempContext.ClientID);
            var handler = player.GetInteractiveHandler();
            if (candidates.Count == 1)
            {
                enemyID = candidates[0];
                return true;
            }
            else
            {
                var result = await handler.SelectEnemy(enemyType);
                enemyID = result.elem;
                return !result.isCancel;
            }
        }
    }
}