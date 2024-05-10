using Data.Animation.Nodes;
using UnityEngine;
using XNodeEditor;

namespace Editor.Animation
{
    [CustomNodeEditor(typeof(PlayProjectileCmd))]
    public class PlayProjectileCmdEditor : NodeEditor
    {
        private PlayProjectileCmd _cmd;

        public override void OnCreate()
        {
            _cmd = target as PlayProjectileCmd;
        }
        
        public override void OnHeaderGUI()
        {
            GUILayout.Label("投射物动画", NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
        }

        public override void OnBodyGUI()
        {
            serializedObject.Update();
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.vfxName)), new GUIContent("特效名称"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.duration)), new GUIContent("持续时间"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.sourceOffset)), new GUIContent("源位置offset"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.targetOffset)), new GUIContent("目标位置offset"));
            serializedObject.ApplyModifiedProperties();
        }
    }
}
