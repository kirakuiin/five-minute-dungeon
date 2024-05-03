using Data.Instruction;
using UnityEngine;
using XNode;

namespace Data.Check
{
    /// <summary>
    /// 检查玩家是否可以执行某个操作。
    /// </summary>
    [CreateAssetMenu(fileName = "CheckGraph", menuName = "检查图", order = 1)]
    public class CheckGraph : NodeGraph
    {
        /// <summary>
        /// 执行蓝图指令。
        /// </summary>
        /// <param name="context"></param>
        /// <param name="clientID"></param>
        public bool Execution(IRuntimeContext context, ulong clientID)
        {
            Debug.Log($"执行蓝图{name}, 客户端ID({clientID})");
            var tempContext = new TempContext(clientID, clientID);
            var result = true;
            foreach (var node in nodes)
            {
                if (node is not CheckBase cmd) continue;
                Debug.Log($"执行节点{cmd.name}");
                result = result && cmd.Execute(context, tempContext);
            }
            Debug.Log($"检查结果为{result}");
            return result;
        }
    }
}
