using Data.Animation.Nodes;
using UnityEngine;
using XNodeEditor;

namespace Editor.Animation
{
    [CustomNodeEditor(typeof(PlayStillVfxCmd))]
    public class PlayStillVfxCmdEditor : NodeEditor
    {
        private PlayStillVfxCmd _cmd;

        public override void OnCreate()
        {
            _cmd = target as PlayStillVfxCmd;
        }
        
        public override void OnHeaderGUI()
        {
            GUILayout.Label("静止动画", NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
        }

        public override void OnBodyGUI()
        {
            serializedObject.Update();
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.vfxName)), new GUIContent("特效名称"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.duration)), new GUIContent("持续时间"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.speed)), new GUIContent("播放速度"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.needAwait)), new GUIContent("需要等待结束"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.targetType)), new GUIContent("特效类型"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.offset)), new GUIContent("目标位置offset"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.rotation)), new GUIContent("特效旋转"));
            serializedObject.ApplyModifiedProperties();
        }
    }
}
