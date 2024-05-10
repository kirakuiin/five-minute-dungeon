using Data.Animation.Nodes;
using UnityEngine;
using XNodeEditor;

namespace Editor.Animation
{
    [CustomNodeEditor(typeof(StopTimeCmd))]
    public class StopTimeCmdEditor : NodeEditor
    {
        private StopTimeCmd _cmd;

        public override void OnCreate()
        {
            _cmd = target as StopTimeCmd;
        }
        
        public override void OnHeaderGUI()
        {
            GUILayout.Label("时停动画", NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
        }

        public override void OnBodyGUI()
        {
            serializedObject.Update();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
