using System.Collections.Generic;
using System.Linq;
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
            InstructionGraph graph = Instantiate(this);
            for (int i = 0; i < nodes.Count; i++) {
                if (nodes[i] == null) continue;
                Node.graphHotfix = graph;
                Node node = Instantiate(nodes[i]) as Node;
                node.name = nodes[i].name;
                node.graph = graph;
                graph.nodes[i] = node;
            }

            // Redirect all connections
            for (int i = 0; i < graph.nodes.Count; i++) {
                if (graph.nodes[i] == null) continue;
                foreach (NodePort port in graph.nodes[i].Ports) {
                    port.Redirect(nodes, graph.nodes);
                }
            }

            graph.subject = subject;
            graph.name = name;
            
            return graph;
        }

    }
}
