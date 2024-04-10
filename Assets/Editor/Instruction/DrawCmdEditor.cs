using Data.Instruction.Nodes;
using UnityEngine;
using XNodeEditor;

namespace Editor.Instruction
{
    [CustomNodeEditor(typeof(DrawCmd))]
    public class DrawCmdEditor : NodeEditor
    {
        private DrawCmd _cmd;

        public override void OnCreate()
        {
            _cmd = target as DrawCmd;
        }
        
        public override void OnHeaderGUI()
        {
            GUILayout.Label("抽牌指令", NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
        }

        public override void OnBodyGUI()
        {
            serializedObject.Update();
            
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.num)), new GUIContent("抽牌数量"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.playerList)), new GUIContent("玩家列表"));
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}
