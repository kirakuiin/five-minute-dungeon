using Data.Instruction.Nodes;
using UnityEngine;
using XNodeEditor;

namespace Editor.Instruction
{
    [CustomNodeEditor(typeof(TimeStopCmd))]
    public class TimeStopCmdEditor: NodeEditor
    {
        private TimeStopCmd _cmd;

        public override void OnCreate()
        {
            _cmd = target as TimeStopCmd;
        }
        
        public override void OnHeaderGUI()
        {
            GUILayout.Label("时停指令", NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
        }
    }
}
