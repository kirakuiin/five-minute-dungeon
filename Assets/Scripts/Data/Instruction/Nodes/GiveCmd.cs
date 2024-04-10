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

        public override async Task Execute(ICmdContext context, TempContext tempContext)
        {
            from = GetInputValue<List<ulong>>(nameof(from));
            to = GetInputValue<List<ulong>>(nameof(to));
            foreach (var player in context.GetPlayerControllers(from))
            {
                player.GiveHand(to.First());
            }
            await Task.CompletedTask;
        }
    }
}