using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using XNode;

namespace Data.Instruction.Nodes
{
    /// <summary>
    /// 选择玩家。
    /// </summary>
    public class SelectPlayerCmd : CommandBase
    {
        /// <summary>
        /// 玩家列表。
        /// </summary>
        [Output] public List<ulong> playerList;
        
        /// <summary>
        /// 玩家目标。
        /// </summary>
        public PlayerTarget target;

        /// <summary>
        /// 选择玩家数量(不能相同)
        /// </summary>
        public int selectPlayerNum;

        public override object GetValue(NodePort port)
        {
            return playerList;
        }
        
        public override async Task Execute(ICmdContext context, TempContext tempContext)
        {
            var player = context.GetPlayerController(tempContext.ClientID);
            playerList = target switch
            {
                PlayerTarget.Self => new List<ulong>() { player.ClientID },
                PlayerTarget.SpecificPlayer => await GetSpecificPlayer(context, player),
                PlayerTarget.AllPlayer => context.GetAllClientIDs().ToList(),
                PlayerTarget.AllExceptSelf => context.GetAllClientIDs().Except(new List<ulong>() {tempContext.ClientID}).ToList(),
                PlayerTarget.AnotherPlayer => await GetAnotherPlayer(context, player),
                _ => playerList
            };
        }

        private async Task<List<ulong>> GetSpecificPlayer(ICmdContext context, IPlayerController player)
        {
            //TODO(nico): 玩家数小于要求数时直接返回结果。
            return await player.GetInteractiveHandler().SelectPlayers(selectPlayerNum, true);
        }
        
        private async Task<List<ulong>> GetAnotherPlayer(ICmdContext context, IPlayerController player)
        {
            //TODO(nico): 玩家数小于要求数时直接返回结果。
            return await player.GetInteractiveHandler().SelectPlayers(selectPlayerNum, false);
        }
    }
}