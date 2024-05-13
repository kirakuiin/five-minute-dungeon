using Data.Animation.Nodes;
using UnityEngine;
using XNodeEditor;

namespace Editor.Animation
{
    [CustomNodeEditor(typeof(DissolveClassCmd))]
    public class DissolveClassCmdEditor : NodeEditor
    {
        private DissolveClassCmd _cmd;

        public override void OnCreate()
        {
            _cmd = target as DissolveClassCmd;
        }
        
        public override void OnHeaderGUI()
        {
            GUILayout.Label("溶解角色动画", NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
        }

        public override void OnBodyGUI()
        {
            serializedObject.Update();
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.playerClass)), new GUIContent("角色职业"));
            serializedObject.ApplyModifiedProperties();
        }
    }
}
