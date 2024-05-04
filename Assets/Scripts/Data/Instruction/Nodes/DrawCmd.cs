using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Data.Instruction.Nodes
{
    /// <summary>
    /// 抽牌。
    /// </summary>
    public class DrawCmd : CommandBase
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
                player.Draw(num);
            }

            await Task.CompletedTask;
            return true;
        }
    }
}