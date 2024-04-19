using Data.Check.Nodes;
using Data.Instruction.Nodes;
using UnityEngine;
using XNodeEditor;

namespace Editor.Check
{
    [CustomNodeEditor(typeof(AlwaysTrueCmd))]
    public class AlwaysTrueCmdEditor : NodeEditor
    {
        private AlwaysTrueCmd _cmd;

        public override void OnCreate()
        {
            _cmd = target as AlwaysTrueCmd;
        }
        
        public override void OnHeaderGUI()
        {
            GUILayout.Label("一直为真", NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
        }

        public override void OnBodyGUI()
        {
            serializedObject.Update();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
