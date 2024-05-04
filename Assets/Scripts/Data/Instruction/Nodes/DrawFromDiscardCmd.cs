using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Instruction.Nodes
{
    /// <summary>
    /// 从弃牌堆顶部抽牌。
    /// </summary>
    public class DrawFromDiscardCmd : CommandBase
    {
        /// <summary>
        /// 玩家列表。
        /// </summary>
        [Input] public List<ulong> playerList;
        
        /// <summary>
        /// 抽牌数量。
        /// </summary>
        public int num;

        public override async Task<bool> Execute(ICmdContext context, TempContext tempContext)
        {
            playerList = GetInputValue<List<ulong>>(nameof(playerList));
            foreach (var player in context.GetPlayerControllers(playerList))
            {
                player.DrawFromDiscard(num);
            }

            await Task.CompletedTask;
            return true;
        }
    }
}