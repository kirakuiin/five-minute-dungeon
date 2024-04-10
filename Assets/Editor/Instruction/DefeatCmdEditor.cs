using Data.Instruction.Nodes;
using UnityEngine;
using XNodeEditor;

namespace Editor.Instruction
{
    [CustomNodeEditor(typeof(DefeatCmd))]
    public class DefeatCmdEditor : NodeEditor
    {
        private DefeatCmd _cmd;

        public override void OnCreate()
        {
            _cmd = target as DefeatCmd;
        }
        
        public override void OnHeaderGUI()
        {
            GUILayout.Label("消灭指令", NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
        }

        public override void OnBodyGUI()
        {
            serializedObject.Update();
            
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("enemyType"), new GUIContent("敌人类型"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("enemyID"), new GUIContent("敌人ID"));
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}
