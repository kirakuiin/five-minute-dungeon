using Data.Animation;
using Data.Instruction.Nodes;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace Editor.Instruction
{
    [CustomNodeEditor(typeof(AnimContextCmd))]
    public class AnimContextCmdEditor: NodeEditor
    {
        private AnimContextCmd _cmd;

        private bool _isShow;

        public override void OnCreate()
        {
            _cmd = target as AnimContextCmd;
        }
        
        public override void OnHeaderGUI()
        {
            GUILayout.Label("动画环境指令", NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
        }

        public override void OnBodyGUI()
        {
            serializedObject.Update();
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.sourceType)), new GUIContent("来源类型"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.targetType)), new GUIContent("目标类型"));
            if (_cmd.targetType != AnimTargetType.Dungeon)
            {
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.targetList)), new GUIContent("目标列表"));
            }
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.animContext)), new GUIContent("动画环境"));
            serializedObject.ApplyModifiedProperties();
        }
    }
}
