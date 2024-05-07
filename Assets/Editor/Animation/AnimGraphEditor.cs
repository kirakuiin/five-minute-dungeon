using Data.Animation;
using Data.Check;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace Editor.Animation
{
    [CustomNodeGraphEditor(typeof(AnimGraph))]
    public class AnimGraphEditor : NodeGraphEditor
    {
        private AnimGraph _graph;

        public override void OnCreate()
        {
            _graph = target as AnimGraph;
        }
    }

    [CustomEditor(typeof(AnimGraph))]
    public class AnimGraphInspector : UnityEditor.Editor
    {
        private AnimGraph _graph;

        private void OnEnable()
        {
            _graph = target as AnimGraph;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_graph.nodes)), new GUIContent("动画序列"));
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}