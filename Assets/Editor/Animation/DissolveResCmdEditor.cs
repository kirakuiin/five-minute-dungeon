using Data.Animation.Nodes;
using UnityEngine;
using XNodeEditor;

namespace Editor.Animation
{
    [CustomNodeEditor(typeof(DissolveResCmd))]
    public class DissolveResCmdEditor : NodeEditor
    {
        private DissolveResCmd _cmd;

        public override void OnCreate()
        {
            _cmd = target as DissolveResCmd;
        }
        
        public override void OnHeaderGUI()
        {
            GUILayout.Label("溶解资源动画", NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
        }

        public override void OnBodyGUI()
        {
            serializedObject.Update();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
