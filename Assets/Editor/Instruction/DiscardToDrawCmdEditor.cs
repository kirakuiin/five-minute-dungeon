using Data.Instruction.Nodes;
using UnityEngine;
using XNodeEditor;

namespace Editor.Instruction
{
    [CustomNodeEditor(typeof(DiscardToDrawCmd))]
    public class DiscardToDrawCmdEditor : NodeEditor
    {
        private DiscardToDrawCmd _cmd;

        public override void OnCreate()
        {
            _cmd = target as DiscardToDrawCmd;
        }
        
        public override void OnHeaderGUI()
        {
            GUILayout.Label("弃牌洗回指令", NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
        }

        public override void OnBodyGUI()
        {
            serializedObject.Update();
            
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.num)), new GUIContent("洗回数量"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.playerList)), new GUIContent("玩家列表"));
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}
