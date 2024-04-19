using Data.Check.Nodes;
using Data.Instruction.Nodes;
using UnityEngine;
using XNodeEditor;

namespace Editor.Check
{
    [CustomNodeEditor(typeof(ExistEnemyCmd))]
    public class ExistEnemyCmdEditor : NodeEditor
    {
        private ExistEnemyCmd _cmd;

        public override void OnCreate()
        {
            _cmd = target as ExistEnemyCmd;
        }
        
        public override void OnHeaderGUI()
        {
            GUILayout.Label("检查敌人类型存在", NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
        }

        public override void OnBodyGUI()
        {
            serializedObject.Update();
            
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.enemyType)), new GUIContent("敌人类型"));
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}
