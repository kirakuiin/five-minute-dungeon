using System.Linq;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
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

        public override object GetValue(NodePort port)
        {
            return enemyID;
        }

        public override async Task Execute(ICmdContext context, TempContext tempContext)
        {
            var player = context.GetPlayerController(tempContext.ClientID);
            var handler = player.GetInteractiveHandler();
            var enemies = context.GetLevelController().GetEnemyIDs().ToList();
            if (enemies.Count == 1)
            {
                enemyID = enemies.First();
            }
            else
            {
                enemyID = await handler.SelectEnemy();
            }
        }
    }
}