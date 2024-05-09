using System.Threading.Tasks;
using UnityEngine;

namespace Data.Animation.Nodes
{
    /// <summary>
    /// 添加资源。
    /// </summary>
    public class PlayLaserCmd: AnimationBase
    {
        [Input] public Color laserColor;

        [Input] public Color subColor;

        public float duration = 0.5f;
        
        public override async Task Execute(IBehaveController controller, AnimContext animContext)
        {
            laserColor = GetInputValue<Color>(nameof(laserColor));
            subColor = GetInputValue<Color>(nameof(subColor));
            var posInfo = controller.GetPositionInfo();
            var from = posInfo.GetAnimTargetPos(animContext.source);
            var to = posInfo.GetARandomNonEventEnemy();
            await controller.GetVfxPlayer().PlayLaser(from, to, duration, laserColor, subColor);
        }
    }
}