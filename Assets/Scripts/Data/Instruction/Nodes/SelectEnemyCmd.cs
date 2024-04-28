using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
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

        public override async Task Execute(ICmdContext context, TempContext tempContext)
        {
            var enemies = context.GetLevelController().GetAllEnemiesInfo();
            var candidates = (from id in enemies.Keys
                where (enemyType & enemies[id].type) > 0
                select id).ToList();
            await SetEnemyID(context, tempContext, candidates);
        }

        private async Task SetEnemyID(ICmdContext context, TempContext tempContext, List<ulong> candidates)
        {
            var player = context.GetPlayerController(tempContext.ClientID);
            var handler = player.GetInteractiveHandler();
            enemyID = candidates.Count switch
            {
                1 => candidates[0],
                > 1 => await handler.SelectEnemy(enemyType),
                _ => EnemyIDDefine.Invalid
            };
        }
    }
}