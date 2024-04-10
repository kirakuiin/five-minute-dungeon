using Data.Instruction.Nodes;
using UnityEngine;
using XNodeEditor;

namespace Editor.Instruction
{
    [CustomNodeEditor(typeof(SelectPlayerCmd))]
    public class SelectPlayerCmdEditor : NodeEditor
    {
        private SelectPlayerCmd _cmd;

        public override void OnCreate()
        {
            _cmd = target as SelectPlayerCmd;
        }

        public override void OnHeaderGUI()
        {
            GUILayout.Label("选择玩家指令", NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
        }

        public override void OnBodyGUI()
        {
            serializedObject.Update();
            
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.target)),
                new GUIContent("玩家目标"));
            if (InstructionHelper.IsNotSelfSinglePlayer(_cmd.target))
            {
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.selectPlayerNum)),
                    new GUIContent("选择数量"));
            }
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.playerList)),
                new GUIContent("玩家列表"));
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}
