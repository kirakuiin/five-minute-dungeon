using Data.Animation.Nodes;
using UnityEngine;
using XNodeEditor;

namespace Editor.Animation
{
    [CustomNodeEditor(typeof(ChangePlayerBackCmd))]
    public class ChangePlayerBackCmdEditor : NodeEditor
    {
        private ChangePlayerBackCmd _cmd;

        public override void OnCreate()
        {
            _cmd = target as ChangePlayerBackCmd;
        }
        
        public override void OnHeaderGUI()
        {
            GUILayout.Label("角色还原", NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
        }

        public override void OnBodyGUI()
        {
            serializedObject.Update();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
