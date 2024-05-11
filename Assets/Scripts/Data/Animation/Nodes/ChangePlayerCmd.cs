using System.Threading.Tasks;
using GameLib.Common;
using UnityEngine;

namespace Data.Animation.Nodes
{
    /// <summary>
    /// 改变玩家指令。
    /// </summary>
    public class ChangePlayerCmd : AnimationBase
    {
        public Vector3 offset;

        public Vector3 localRotation = Vector3.zero;

        public ModelChangeMode changeMode = ModelChangeMode.Instantly;

        public float changeTime;
        
        public override async Task Execute(IBehaveController controller, AnimContext animContext)
        {
            if (animContext.targets.Count == 1)
            {
                var pos = controller.GetPositionInfo().GetAnimTargetPos(animContext.targets[0]);
                controller.GetModelPlayer(animContext.source).ChangeTo(
                    new ModelChangeParam
                    {
                        position = pos+offset,
                        localRot = localRotation,
                        changeTime = changeTime,
                        mode = changeMode
                    });
                if (changeMode == ModelChangeMode.LinerInterpolation)
                {
                    await Task.Delay(TimeScalar.ConvertSecondToMs(changeTime));
                }
            }
        }
    }
}