using Data.Check.Nodes;
using Data.Instruction.Nodes;
using UnityEngine;
using XNodeEditor;

namespace Editor.Check
{
    [CustomNodeEditor(typeof(HandGreaterThanCmd))]
    public class HandGreaterThanCmdEditor : NodeEditor
    {
        private HandGreaterThanCmd _cmd;

        public override void OnCreate()
        {
            _cmd = target as HandGreaterThanCmd;
        }
        
        public override void OnHeaderGUI()
        {
            GUILayout.Label("检查手牌大小指令", NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
        }

        public override void OnBodyGUI()
        {
            serializedObject.Update();
            
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.handSize)), new GUIContent("手牌大小"));
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}
