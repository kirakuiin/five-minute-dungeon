using System.Threading.Tasks;
using UnityEngine;
using XNode;

namespace Data.Animation
{
    /// <summary>
    /// 动画播放序列。
    /// </summary>
    [CreateAssetMenu(fileName = "AnimGraph", menuName = "动画图", order = 2)]
    public class AnimGraph : NodeGraph
    {
        /// <summary>
        /// 执行动画序列。
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="animContext"></param>
        public async Task Execution(IBehaveController controller, AnimContext animContext)
        {
            Debug.Log($"执行动画{name}");
            foreach (var node in nodes)
            {
                if (node is not AnimationBase cmd) continue;
                Debug.Log($"执行节点{cmd.name}");
                await cmd.Execute(controller, animContext);
            }
        }
    }
}
