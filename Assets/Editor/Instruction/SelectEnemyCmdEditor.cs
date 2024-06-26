using Data.Instruction.Nodes;
using UnityEngine;
using XNodeEditor;

namespace Editor.Instruction
{
    [CustomNodeEditor(typeof(SelectEnemyCmd))]
    public class SelectEnemyCmdEditor: NodeEditor
    {
        private SelectEnemyCmd _cmd;

        public override void OnCreate()
        {
            _cmd = target as SelectEnemyCmd;
        }

        public override void OnHeaderGUI()
        {
            GUILayout.Label("选择敌人指令", NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
        }

        public override void OnBodyGUI()
        {
            serializedObject.Update();
            
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.enemyID)), new GUIContent("敌人ID"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.enemyType)), new GUIContent("敌人类型"));
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}
