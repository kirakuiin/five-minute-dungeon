using System.Linq;
using System.Threading.Tasks;
using Vector3 = UnityEngine.Vector3;

namespace Data.Animation.Nodes
{
    /// <summary>
    /// 播放投射物。
    /// </summary>
    public class PlayProjectileCmd : AnimationBase
    {
        public float duration;

        public string vfxName;
        
        public Vector3 sourceOffset = Vector3.zero;
        
        public Vector3 targetOffset = Vector3.zero;
        
        public override async Task Execute(IBehaveController controller, AnimContext animContext)
        {
            var posInfo = controller.GetPositionInfo();
            var from = posInfo.GetAnimTargetPos(animContext.source);
            var posList = animContext.targets.Select(target => posInfo.GetAnimTargetPos(target));
            await Task.WhenAll(posList.Select(to =>
                controller.GetVfxPlayer().PlayProjectile(vfxName, from+sourceOffset, to+targetOffset, duration)));
        }
    }
}