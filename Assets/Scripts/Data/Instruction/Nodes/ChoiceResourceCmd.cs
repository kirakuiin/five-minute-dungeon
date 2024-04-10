using System;
using XNode;
using System.Threading.Tasks;
using Common;
using GameLib.Common.Extension;
using UnityEngine;

namespace Data.Instruction.Nodes
{
    /// <summary>
    /// 玩家挑选一种资源。（用于多人投票）
    /// </summary>
    public class ChoiceResourceCmd : CommandBase
    {
        /// <summary>
        /// 挑选的资源。
        /// </summary>
        [Output] public Resource resource;
        
        public override object GetValue(NodePort port)
        {
            return resource;
        }
        
        public override async Task Execute(ICmdContext context, TempContext tmpContext)
        {
            var player = context.GetPlayerController(tmpContext.ClientID);
            resource = await player.GetInteractiveHandler().SelectResource();
            await UseMostChoiceResource(tmpContext);
        }
        
        private async Task UseMostChoiceResource(TempContext tmpContext)
        {
            tmpContext.Group.AddChoice(resource);
            await TaskExtension.Wait(() => tmpContext.Group.IsMakeDecision);
            resource = tmpContext.Group.GetMostResource();
            Debug.Log($"最多决定{resource}");
        }
    }
}