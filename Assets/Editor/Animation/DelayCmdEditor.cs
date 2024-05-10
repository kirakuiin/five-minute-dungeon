using Data.Animation.Nodes;
using UnityEngine;
using XNodeEditor;

namespace Editor.Animation
{
    [CustomNodeEditor(typeof(DelayCmd))]
    public class DelayCmdEditor : NodeEditor
    {
        private DelayCmd _cmd;

        public override void OnCreate()
        {
            _cmd = target as DelayCmd;
        }
        
        public override void OnHeaderGUI()
        {
            GUILayout.Label("延时", NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
        }

        public override void OnBodyGUI()
        {
            serializedObject.Update();
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.delay)), new GUIContent("延迟"));
            serializedObject.ApplyModifiedProperties();
        }
    }
}
