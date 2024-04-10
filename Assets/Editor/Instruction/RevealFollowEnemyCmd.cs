using Data.Instruction.Nodes;
using UnityEngine;
using XNodeEditor;

namespace Editor.Instruction
{
    [CustomNodeEditor(typeof(RevealFollowEnemyCmd))]
    public class RevealFollowEnemyCmdEditor: NodeEditor
    {
        private RevealFollowEnemyCmd _cmd;

        public override void OnCreate()
        {
            _cmd = target as RevealFollowEnemyCmd;
        }
        
        public override void OnHeaderGUI()
        {
            GUILayout.Label("揭露敌人指令", NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
        }

        public override void OnBodyGUI()
        {
            serializedObject.Update();
            
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.num)), new GUIContent("揭露数量"));
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}
