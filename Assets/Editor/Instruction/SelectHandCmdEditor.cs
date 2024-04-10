using Data.Instruction.Nodes;
using UnityEngine;
using XNodeEditor;

namespace Editor.Instruction
{
    [CustomNodeEditor(typeof(SelectHandCmd))]
    public class SelectHandCmdEditor : NodeEditor
    {
        private SelectHandCmd _cmd;

        public override void OnCreate()
        {
            _cmd = target as SelectHandCmd;
        }

        public override void OnHeaderGUI()
        {
            GUILayout.Label("选择手牌指令", NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
        }

        public override void OnBodyGUI()
        {
            serializedObject.Update();
            
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.playerList)),
                new GUIContent("玩家列表"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.cardList)),
                new GUIContent("选择手牌"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.num)),
                new GUIContent("选择数量"));
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}
