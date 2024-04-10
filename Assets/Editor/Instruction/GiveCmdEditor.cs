using Data.Instruction.Nodes;
using UnityEngine;
using XNodeEditor;

namespace Editor.Instruction
{
    [CustomNodeEditor(typeof(GiveCmd))]
    public class GiveCmdEditor: NodeEditor
    {
        private GiveCmd _cmd;

        public override void OnCreate()
        {
            _cmd = target as GiveCmd;
        }
        
        public override void OnHeaderGUI()
        {
            GUILayout.Label("给牌指令", NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
        }

        public override void OnBodyGUI()
        {
            serializedObject.Update();
            
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.from)), new GUIContent("来源"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.to)), new GUIContent("目标"));
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}
