using Data.Instruction;
using UnityEngine;
using XNode;

namespace Data.Check
{
    [CreateAssetMenu(fileName = "CheckGraph", menuName = "检查图", order = 1)]
    public class CheckGraph : NodeGraph
    {
        /// <summary>
        /// 执行蓝图指令。
        /// </summary>
        /// <param name="context"></param>
        /// <param name="subjectID"></param>
        public bool Execution(IRuntimeContext context, ulong subjectID)
        {
            Debug.Log($"执行蓝图{name}, 主体ID({subjectID})");
            var tempContext = new TempContext(subjectID);
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
