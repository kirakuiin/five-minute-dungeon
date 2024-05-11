using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Data.Animation.Nodes
{
    /// <summary>
    /// 播放静止特效
    /// </summary>
    public class PlayStillVfxCmd : AnimationBase
    {
        public string vfxName;
        
        public float duration;

        public float speed = 1.0f;
        
        public Vector3 rotation = Vector3.zero;

        public bool needAwait;
        
        public StillVfxTargetType targetType;

        public Vector3 offset;

        public override async Task Execute(IBehaveController controller, AnimContext animContext)
        {
            var posInfo = controller.GetPositionInfo();
            var posList = targetType switch
            {
                StillVfxTargetType.Source => new List<Vector3> {posInfo.GetAnimTargetPos(animContext.source)},
                StillVfxTargetType.Target => animContext.targets.Select(target => posInfo.GetAnimTargetPos(target)).ToList(),
                StillVfxTargetType.EnemyCenter => new List<Vector3> {posInfo.GetEnemyCenter()},
                StillVfxTargetType.PlayerCenter => new List<Vector3> {posInfo.GetPlayerCenter()},
                StillVfxTargetType.Center => new List<Vector3> {posInfo.GetEnemyCenter()/2 + posInfo.GetPlayerCenter()/2},
                _ => throw new ArgumentOutOfRangeException()
            };
            await Task.WhenAll(posList.Select(pos =>
                controller.GetVfxPlayer().PlayStillVfx(vfxName,
                    new StillVfxParam
                    {
                        target = pos + offset,
                        duration = duration,
                        rotation = rotation,
                        speed = speed,
                        needAwait = needAwait
                    })));
        }
    }
}