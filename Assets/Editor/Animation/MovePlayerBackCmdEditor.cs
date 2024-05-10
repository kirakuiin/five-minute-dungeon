using Data.Animation.Nodes;
using UnityEngine;
using XNodeEditor;

namespace Editor.Animation
{
    [CustomNodeEditor(typeof(MovePlayerBackCmd))]
    public class MovePlayerBackCmdEditor : NodeEditor
    {
        private MovePlayerBackCmd _cmd;

        public override void OnCreate()
        {
            _cmd = target as MovePlayerBackCmd;
        }
        
        public override void OnHeaderGUI()
        {
            GUILayout.Label("角色返回原位", NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
        }

        public override void OnBodyGUI()
        {
            serializedObject.Update();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
