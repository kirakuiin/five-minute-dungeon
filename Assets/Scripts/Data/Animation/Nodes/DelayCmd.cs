using System.Threading.Tasks;
using GameLib.Common;

namespace Data.Animation.Nodes
{
    /// <summary>
    /// 延时动画。
    /// </summary>
    public class DelayCmd : AnimationBase
    {
        public float delay;
        
        public override async Task Execute(IBehaveController controller, AnimContext animContext)
        {
            await Task.Delay(TimeScalar.ConvertSecondToMs(delay));
        }
    }
}