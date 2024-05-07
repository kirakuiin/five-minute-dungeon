using Data.Animation.Nodes;
using UnityEngine;
using XNodeEditor;

namespace Editor.Animation
{
    [CustomNodeEditor(typeof(PlayLaserCmd))]
    public class PlayLaserCmdEditor : NodeEditor
    {
        private PlayLaserCmd _cmd;

        public override void OnCreate()
        {
            _cmd = target as PlayLaserCmd;
        }
        
        public override void OnHeaderGUI()
        {
            GUILayout.Label("发射激光", NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
        }

        public override void OnBodyGUI()
        {
            serializedObject.Update();
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.laserColor)), new GUIContent("激光主颜色"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.subColor)), new GUIContent("激光次颜色"));
            serializedObject.ApplyModifiedProperties();
        }
    }
}
