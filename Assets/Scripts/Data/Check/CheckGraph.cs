using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Instruction;
using Data.Instruction.Nodes;
using UnityEngine;
using XNode;

namespace Data.Check
{
    [CreateAssetMenu(fileName = "CheckGraph", menuName = "检查图", order = 1)]
    public class CheckGraph : NodeGraph
    {
        /// <summary>
        /// 发起此指令的主体。
        /// </summary>
        public InstructionSubject subject;

        /// <summary>
        /// 执行蓝图指令。
        /// </summary>
        /// <param name="context"></param>
        /// <param name="subjectID"></param>
        public async Task Execution(ICmdContext context, ulong subjectID)
        {
            Debug.Log($"执行蓝图{name}, 主体ID({subjectID})");
            if (subject == InstructionSubject.AllPlayer)
            {
                await ExecuteForAllClient(context);
            }
            else
            {
                await InnerExecution(context, new TempContext(subjectID));
            }
        }

        private async Task ExecuteForAllClient(ICmdContext context)
        {
            var taskPool = new List<Task>();
            var contexts = new TempContextGroup(context.GetAllClientIDs());

            foreach (var tempContext in contexts)
            {
                taskPool.Add(InnerExecution(context, tempContext));
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
                await cmd.Execute(context, tmpContext);
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

        public override NodeGraph Copy()
        {
            var graph = base.Copy() as InstructionGraph;
            if (!graph) return graph;
            graph.subject = subject;
            graph.name = name;
            return graph;
        }

    }
}
