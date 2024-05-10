using Data.Animation.Nodes;
using UnityEngine;
using XNodeEditor;

namespace Editor.Animation
{
    [CustomNodeEditor(typeof(MovePlayerCmd))]
    public class MovePlayerCmdEditor : NodeEditor
    {
        private MovePlayerCmd _cmd;

        public override void OnCreate()
        {
            _cmd = target as MovePlayerCmd;
        }
        
        public override void OnHeaderGUI()
        {
            GUILayout.Label("位移角色", NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
        }

        public override void OnBodyGUI()
        {
            serializedObject.Update();
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.offset)), new GUIContent("偏移"));
            serializedObject.ApplyModifiedProperties();
        }
    }
}
