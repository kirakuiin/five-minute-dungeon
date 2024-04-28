using Data.Check.Nodes;
using Data.Instruction.Nodes;
using UnityEngine;
using XNodeEditor;

namespace Editor.Check
{
    [CustomNodeEditor(typeof(NeedResCmd))]
    public class NeedResCmdEditor : NodeEditor
    {
        private NeedResCmd _cmd;

        public override void OnCreate()
        {
            _cmd = target as NeedResCmd;
        }
        
        public override void OnHeaderGUI()
        {
            GUILayout.Label("检查是否需要资源", NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
        }

        public override void OnBodyGUI()
        {
            serializedObject.Update();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
