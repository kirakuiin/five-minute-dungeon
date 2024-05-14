using Data.Animation.Nodes;
using UnityEngine;
using XNodeEditor;

namespace Editor.Animation
{
    [CustomNodeEditor(typeof(PlayAnimCmd))]
    public class PlayAnimCmdEditor : NodeEditor
    {
        private PlayAnimCmd _cmd;

        public override void OnCreate()
        {
            _cmd = target as PlayAnimCmd;
        }
        
        public override void OnHeaderGUI()
        {
            GUILayout.Label("模型动画", NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
        }

        public override void OnBodyGUI()
        {
            serializedObject.Update();
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.animName)), new GUIContent("动画名"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.speed)), new GUIContent("动画速度"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.isSource)), new GUIContent("是否为来源动画"));
            serializedObject.ApplyModifiedProperties();
        }
    }
}
