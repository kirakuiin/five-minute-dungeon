﻿using System.Threading.Tasks;

namespace Data.Animation.Nodes
{
    /// <summary>
    /// 溶解资源指令。
    /// </summary>
    public class DissolveResCmd : AnimationBase
    {
        public override async Task Execute(IBehaveController controller, AnimContext animContext)
        {
            await controller.GetVfxPlayer().PlayDissolveRes(animContext.other.selectedRes);
        }
    }
}