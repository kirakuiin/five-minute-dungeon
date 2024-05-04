using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Instruction.Nodes;
using UnityEngine;
using XNode;

namespace Data.Instruction
{
    [CreateAssetMenu(fileName = "InstructionGraph", menuName = "指令图", order = 0)]
    public class InstructionGraph : NodeGraph
    {
        /// <summary>
        /// 发起此指令的主体。
        /// </summary>
        public InstructionSubject subject;

        /// <summary>
        /// 执行蓝图指令。
        /// </summary>
        /// <param name="context"></param>
        /// <param name="subjectID">蓝图释放主体ID</param>
        /// <param name="clientID">执行指令的客户端ID</param>
        public async Task Execution(ICmdContext context, ulong subjectID, ulong clientID)
        {
            Debug.Log($"执行蓝图{name}, 主体ID({subjectID})");
            if (subject == InstructionSubject.AllPlayer)
            {
                await ExecuteForAllClient(subjectID, context);
            }
            else
            {
                await InnerExecution(context, new TempContext(subjectID, clientID));
            }
        }

        private async Task ExecuteForAllClient(ulong subjectId, ICmdContext context)
        {
            var taskPool = new List<Task>();
            var contexts = new TempContextGroup(subjectId, context.GetAllClientIDs());

            foreach (var tempContext in contexts)
            {
                var clone = Clone();
                taskPool.Add(clone.InnerExecution(context, tempContext));
            }

            await Task.WhenAll(taskPool);
        }

        private async Task InnerExecution(ICmdContext context, TempContext tmpContext)
        {
            Debug.Log($"临时上下文: {tmpContext}");
            foreach (var node in nodes)
            {
                if (node is not CommandBase cmd) continue;
                Debug.Log($"执行节点{cmd.name}");
                var isSuccess = await cmd.Execute(context, tmpContext);
                if (!isSuccess)
                {
                    Debug.Log($"取消节点{cmd.name}执行");
                    break;
                }
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// 获得所有的资源节点的信息。
        /// </summary>
        /// <returns></returns>
        public Dictionary<Resource, int> GetAllResourceInfo()
        {
            var result = new Dictionary<Resource, int>();
            foreach (var node in nodes)
            {
                if (node is AddResourceCmd cmd)
                {
                    result[cmd.resource] = result.TryGetValue(cmd.resource, out var cur) ? cur + cmd.num : cmd.num;
                }
            }

            return result;
        }

        private InstructionGraph Clone()
        {
            var clone = Copy() as InstructionGraph;
            if (!clone) return clone;

            clone.subject = subject;
            clone.name = name;
            return clone;
        }
    }
}
