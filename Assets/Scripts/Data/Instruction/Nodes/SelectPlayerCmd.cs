﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        
        public override async Task<bool> Execute(ICmdContext context, TempContext tempContext)
        {
            // 处理玩家掉线的特殊情况。
            if (!context.GetAllClientIDs().Contains(tempContext.ClientID))
            {
                return false;
            }
            
            var player = context.GetPlayerController(tempContext.ClientID);
            playerList = target switch
            {
                PlayerTarget.Self => new List<ulong>() { player.ClientID },
                PlayerTarget.SpecificPlayer => await GetPlayerList(context, player, true),
                PlayerTarget.AllPlayer => context.GetAllClientIDs().ToList(),
                PlayerTarget.AllExceptSelf => context.GetAllClientIDs().Except(new List<ulong>() {tempContext.ClientID}).ToList(),
                PlayerTarget.AnotherPlayer => await GetPlayerList(context, player, false),
                _ => playerList
            };
            return playerList.Count != 0;
        }

        private async Task<List<ulong>> GetPlayerList(ICmdContext context, IPlayerController player, bool canSelectSelf)
        {
            var allClientIDs = context.GetAllClientIDs().ToList();
            if (!canSelectSelf)
            {
                allClientIDs.Remove(player.ClientID);
            }
            if (allClientIDs.Count <= selectPlayerNum)
            {
                return allClientIDs;
            }
            return await player.GetInteractiveHandler().SelectPlayers(selectPlayerNum, canSelectSelf);
        }
    }
}