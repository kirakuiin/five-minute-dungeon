using System.Threading.Tasks;
using XNode;

namespace Data.Animation
{
    /// <summary>
    /// 动画基础节点。
    /// </summary>
    public abstract class AnimationBase : Node
    {
        /// <summary>
        /// 执行指令。
        /// </summary>
        /// <param name="controller">执行动画所需要的表现上下文。</param>
        /// <param name="animContext">执行动画所需要的动画上下文。</param>
        /// <returns></returns>
        public abstract Task Execute(IBehaveController controller, AnimContext animContext);
    }
}