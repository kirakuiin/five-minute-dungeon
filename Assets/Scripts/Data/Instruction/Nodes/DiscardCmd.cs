using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Instruction.Nodes
{
    /// <summary>
    /// 弃牌。
    /// </summary>
    public class DiscardCmd : CommandBase
    {
        /// <summary>
        /// 玩家列表。
        /// </summary>
        [Input] public List<ulong> playerList;

        /// <summary>
        /// 弃牌列表。
        /// </summary>
        [Input] public List<Card> discardList;

        public override async Task<bool> Execute(ICmdContext context, TempContext tempContext)
        {
            playerList = GetInputValue<List<ulong>>(nameof(playerList));
            discardList = GetInputValue<List<Card>>(nameof(discardList));
            var player = context.GetPlayerController(playerList[0]);
            player.Discard(discardList);
            await Task.CompletedTask;
            return true;
        }
    }
}