using Data.Instruction.Nodes;
using UnityEngine;
using XNodeEditor;

namespace Editor.Instruction
{
    [CustomNodeEditor(typeof(AddResourceCmd))]
    public class AddResourceCmdEditor: NodeEditor
    {
        private AddResourceCmd _cmd;

        public override void OnCreate()
        {
            _cmd = target as AddResourceCmd;
        }
        
        public override void OnHeaderGUI()
        {
            GUILayout.Label("添加资源指令", NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
        }

        public override void OnBodyGUI()
        {
            serializedObject.Update();
            
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.resource)), new GUIContent("资源类型"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.num)), new GUIContent("资源数量"));
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}
