using Data.Instruction.Nodes;
using UnityEngine;
using XNodeEditor;

namespace Editor.Instruction
{
    [CustomNodeEditor(typeof(DiscardCmd))]
    public class DiscardCmdEditor : NodeEditor
    {
        private DiscardCmd _cmd;

        public override void OnCreate()
        {
            _cmd = target as DiscardCmd;
        }
        
        public override void OnHeaderGUI()
        {
            GUILayout.Label("弃牌指令", NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
        }

        public override void OnBodyGUI()
        {
            serializedObject.Update();
            
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.playerList)), new GUIContent("玩家列表"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.discardList)), new GUIContent("弃牌列表"));
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}
