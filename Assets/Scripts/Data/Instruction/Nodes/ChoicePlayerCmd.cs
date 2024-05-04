using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameLib.Common.Extension;
using UnityEngine;
using XNode;

namespace Data.Instruction.Nodes
{
    /// <summary>
    /// 挑选单个玩家目标（用于多人投票）
    /// </summary>
    public class ChoicePlayerCmd : CommandBase
    {
        [Output] public List<ulong> playerList;

        public override object GetValue(NodePort port)
        {
            return playerList;
        }

        public override async Task<bool> Execute(ICmdContext context, TempContext tmpContext)
        {
            var player = context.GetPlayerController(tmpContext.ClientID);
            var allPlayerIDs = context.GetAllClientIDs().ToList();
            playerList = allPlayerIDs.Count switch
            {
                1 => allPlayerIDs,
                _ => await player.GetInteractiveHandler().SelectPlayers(1, true),
            };
            await UseMostChoicePlayer(tmpContext);
            return true;
        }

        private async Task UseMostChoicePlayer(TempContext tmpContext)
        {
            tmpContext.Group.AddChoice(playerList[0]);
            await TaskExtension.Wait(() => tmpContext.Group.IsMakeDecision);
            playerList[0] = tmpContext.Group.GetMostPlayer();
            Debug.Log($"最多决定{playerList[0]}");
        }
    }
}