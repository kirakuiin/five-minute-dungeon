using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Instruction.Nodes
{
    /// <summary>
    /// 弃牌堆到抽牌堆。
    /// </summary>
    public class DiscardToDrawCmd : CommandBase
    {
        /// <summary>
        /// 玩家列表。
        /// </summary>
        [Input] public List<ulong> playerList;

        /// <summary>
        /// 洗回数量。
        /// </summary>
        public int num;

        public override async Task<bool> Execute(ICmdContext context, TempContext tempContext)
        {
            playerList = GetInputValue<List<ulong>>(nameof(playerList));
            foreach (var player in context.GetPlayerControllers(playerList))
            {
                player.DiscardToDraw(num);
            }

            await Task.CompletedTask;
            return true;
        }
    }
}