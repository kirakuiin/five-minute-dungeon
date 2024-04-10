using Data.Instruction;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace Editor.Instruction
{
    [NodeGraphEditor.CustomNodeGraphEditor(typeof(InstructionGraph))]
    public class InstructionGraphEditor : NodeGraphEditor
    {
        private InstructionGraph _graph;

        public override void OnCreate()
        {
            _graph = target as InstructionGraph;
        }
    }

    [CustomEditor(typeof(InstructionGraph))]
    public class InstructGraphInspector : UnityEditor.Editor
    {
        private InstructionGraph _graph;

        private void OnEnable()
        {
            _graph = target as InstructionGraph;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_graph.nodes)), new GUIContent("指令列表"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_graph.subject)), new GUIContent("执行主体"));
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}