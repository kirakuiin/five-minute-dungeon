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
        
        /* 实现动态端口的UI显示。
        public override void OnBodyGUI()
        {
            serializedObject.Update();
            NodeEditorGUILayout.DynamicPortList(nameof(_cmd.targetList), typeof(AnimTarget), serializedObject,
                NodePort.IO.Input, onCreation: OnCreateList);
            serializedObject.ApplyModifiedProperties();
        }

        private void OnCreateList(ReorderableList list)
        {
            list.drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "目标列表");
            };
            list.drawElementCallback = DrawElementCallback;
            return;

            void DrawElementCallback(Rect rect, int index, bool active, bool focused)
            {
                var property = serializedObject.FindProperty(nameof(_cmd.targetList)).GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, 80, EditorGUIUtility.singleLineHeight),
                    property.FindPropertyRelative("type"), GUIContent.none);
                EditorGUI.LabelField(new Rect(rect.x+90, rect.y, 20, EditorGUIUtility.singleLineHeight), "ID:");
                EditorGUI.PropertyField(new Rect(rect.x+110, rect.y, 30, EditorGUIUtility.singleLineHeight),
                    property.FindPropertyRelative("id"), GUIContent.none);
                var port = _cmd.GetPort(nameof(_cmd.targetList) + " " + index);
                var pos = rect.position + (port.IsOutput ? new Vector2(rect.width + 6, 0) : new Vector2(-36, 0));
                NodeEditorGUILayout.PortField(pos, port);
            }
        }
        */
    }
}
