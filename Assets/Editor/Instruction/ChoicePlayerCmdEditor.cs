using Data.Instruction.Nodes;
using UnityEngine;
using XNodeEditor;

namespace Editor.Instruction
{
    [CustomNodeEditor(typeof(ChoicePlayerCmd))]
    public class ChoicePlayerCmdEditor : NodeEditor
    {
        private ChoicePlayerCmd _cmd;

        public override void OnCreate()
        {
            _cmd = target as ChoicePlayerCmd;
        }

        public override void OnHeaderGUI()
        {
            GUILayout.Label("挑选玩家指令", NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
        }

        public override void OnBodyGUI()
        {
            serializedObject.Update();
            
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.playerList)),
                new GUIContent("挑选玩家"));
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}
