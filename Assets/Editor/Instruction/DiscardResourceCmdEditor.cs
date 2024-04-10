using Data.Instruction.Nodes;
using UnityEngine;
using XNodeEditor;

namespace Editor.Instruction
{
    [CustomNodeEditor(typeof(DiscardResourceCmd))]
    public class DiscardResourceCmdEditor : NodeEditor
    {
        private DiscardResourceCmd _cmd;

        public override void OnCreate()
        {
            _cmd = target as DiscardResourceCmd;
        }
        
        public override void OnHeaderGUI()
        {
            GUILayout.Label("丢弃资源指令", NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
        }

        public override void OnBodyGUI()
        {
            serializedObject.Update();
            
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.resource)), new GUIContent("丢弃资源"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.playerList)), new GUIContent("玩家列表"));
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}
