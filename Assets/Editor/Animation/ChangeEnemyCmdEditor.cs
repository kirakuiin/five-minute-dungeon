using Data.Animation;
using Data.Animation.Nodes;
using UnityEngine;
using XNodeEditor;

namespace Editor.Animation
{
    [CustomNodeEditor(typeof(ChangeEnemyCmd))]
    public class ChangeEnemyCmdEditor : NodeEditor
    {
        private ChangeEnemyCmd _cmd;

        public override void OnCreate()
        {
            _cmd = target as ChangeEnemyCmd;
        }
        
        public override void OnHeaderGUI()
        {
            GUILayout.Label("改变敌人", NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
        }

        public override void OnBodyGUI()
        {
            serializedObject.Update();
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.offset)), new GUIContent("偏移"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.localRotation)), new GUIContent("局部旋转"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.changeMode)), new GUIContent("变化模式"));
            if (_cmd.changeMode == ModelChangeMode.LinerInterpolation)
            {
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.changeTime)), new GUIContent("时间"));
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
