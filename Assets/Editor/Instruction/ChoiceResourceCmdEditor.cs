using Data.Instruction.Nodes;
using UnityEngine;
using XNodeEditor;

namespace Editor.Instruction
{
    [CustomNodeEditor(typeof(ChoiceResourceCmd))]
    public class ChoiceResourceCmdEditor : NodeEditor
    {
        private ChoiceResourceCmd _cmd;

        public override void OnCreate()
        {
            _cmd = target as ChoiceResourceCmd;
        }

        public override void OnHeaderGUI()
        {
            GUILayout.Label("挑选资源指令", NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
        }

        public override void OnBodyGUI()
        {
            serializedObject.Update();
            
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.resource)),
                new GUIContent("资源"));
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}
