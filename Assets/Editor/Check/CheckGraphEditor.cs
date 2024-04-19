using Data.Check;
using Data.Instruction;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace Editor.Check
{
    [CustomNodeGraphEditor(typeof(CheckGraph))]
    public class CheckGraphEditor : NodeGraphEditor
    {
        private CheckGraph _graph;

        public override void OnCreate()
        {
            _graph = target as CheckGraph;
        }
    }

    [CustomEditor(typeof(CheckGraph))]
    public class CheckGraphInspector : UnityEditor.Editor
    {
        private CheckGraph _graph;

        private void OnEnable()
        {
            _graph = target as CheckGraph;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_graph.nodes)), new GUIContent("指令列表"));
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}