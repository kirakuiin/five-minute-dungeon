using Data.Animation.Nodes;
using UnityEngine;
using XNodeEditor;

namespace Editor.Animation
{
    [CustomNodeEditor(typeof(ResColorMixerCmd))]
    public class ResColorMixerCmdEditor : NodeEditor
    {
        private ResColorMixerCmd _cmd;

        public override void OnCreate()
        {
            _cmd = target as ResColorMixerCmd;
        }
        
        public override void OnHeaderGUI()
        {
            GUILayout.Label("混合资源颜色", NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
        }

        public override void OnBodyGUI()
        {
            serializedObject.Update();
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.resList)), new GUIContent("资源列表"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.color)), new GUIContent("主颜色"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.subColor)), new GUIContent("次颜色"));
            serializedObject.ApplyModifiedProperties();
        }
    }
}
