using System.Threading.Tasks;
using GameLib.Common;
using UnityEngine;
using UnityEngine.Serialization;

namespace Data.Animation.Nodes
{
    /// <summary>
    /// 改变敌人指令。
    /// </summary>
    public class ChangeEnemyCmd : AnimationBase
    {
        public Vector3 offset;

        public Vector3 localRotation = Vector3.zero;

        public ModelChangeMode changeMode = ModelChangeMode.Instantly;

        public float changeTime;
        
        public override async Task Execute(IBehaveController controller, AnimContext animContext)
        {
            foreach (var target in animContext.targets)
            {
                var targetPos = controller.GetPositionInfo().GetAnimTargetPos(target);
                controller.GetModelPlayer(target).ChangeTo(
                    new ModelChangeParam
                    {
                        position = targetPos + offset,
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