using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Data.Instruction.Nodes
{
    /// <summary>
    /// 丢弃指定的的资源。
    /// </summary>
    public class DiscardResourceCmd : CommandBase
    {
        /// <summary>
        /// 玩家列表。
        /// </summary>
        [Input] public List<ulong> playerList;
        
        /// <summary>
        /// 丢弃的资源。
        /// </summary>
        [Input] public Resource resource;

        public override async Task Execute(ICmdContext context, TempContext tempContext)
        {
            playerList = GetInputValue<List<ulong>>(nameof(playerList));
            resource = GetInputValue<Resource>(nameof(resource));
            foreach (var player in context.GetPlayerControllers(playerList))
            {
                player.DiscardResource(resource);
            }
            await Task.CompletedTask;
        }
    }
}