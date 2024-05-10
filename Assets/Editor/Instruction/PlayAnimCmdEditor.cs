using Data.Instruction.Nodes;
using UnityEngine;
using XNodeEditor;

namespace Editor.Instruction
{
    [CustomNodeEditor(typeof(PlayAnimCmd))]
    public class PlayAnimCmdEditor: NodeEditor
    {
        private PlayAnimCmd _cmd;

        private bool _isShow;

        public override void OnCreate()
        {
            _cmd = target as PlayAnimCmd;
        }
        
        public override void OnHeaderGUI()
        {
            GUILayout.Label("播放动画指令", NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
        }

        public override void OnBodyGUI()
        {
            serializedObject.Update();
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.animContext)), new GUIContent("动画环境"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.animGraph)), new GUIContent("动画序列"));
            serializedObject.ApplyModifiedProperties();
        }
    }
}
