using Data.Animation.Nodes;
using UnityEngine;
using XNodeEditor;

namespace Editor.Animation
{
    [CustomNodeEditor(typeof(PlayAudioCmd))]
    public class PlayAudioCmdEditor : NodeEditor
    {
        private PlayAudioCmd _cmd;

        public override void OnCreate()
        {
            _cmd = target as PlayAudioCmd;
        }
        
        public override void OnHeaderGUI()
        {
            GUILayout.Label("播放音效", NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
        }

        public override void OnBodyGUI()
        {
            serializedObject.Update();
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.clipName)), new GUIContent("音效名称"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.volume)), new GUIContent("音量"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.isGlobal)), new GUIContent("是否为全局音效"));
            if (!_cmd.isGlobal)
            {
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.isOnSource)), new GUIContent("是否在源上播放"));
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
