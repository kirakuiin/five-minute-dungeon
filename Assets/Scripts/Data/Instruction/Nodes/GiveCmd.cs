using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Instruction.Nodes
{
    /// <summary>
    /// 给予手牌。
    /// </summary>
    public class GiveCmd : CommandBase
    {
        /// <summary>
        /// 来源。
        /// </summary>
        [Input] public List<ulong> from;
        
        /// <summary>
        /// 目标。
        /// </summary>
        [Input] public List<ulong> to;

        public override async Task<bool> Execute(ICmdContext context, TempContext tempContext)
        {
            from = GetInputValue<List<ulong>>(nameof(from));
            to = GetInputValue<List<ulong>>(nameof(to));
            if (to.Count == 1)
            {
                var targetID = to.First();
                foreach (var player in context.GetPlayerControllers(from))
                {
                    if (player.ClientID == targetID) continue;
                    player.GiveHand(to.First());
                }
            }
            await Task.CompletedTask;
            return true;
        }
    }
}