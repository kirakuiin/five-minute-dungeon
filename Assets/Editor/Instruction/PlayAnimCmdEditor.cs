using Data.Animation;
using Data.Instruction.Nodes;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using XNode;
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
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.sourceType)), new GUIContent("来源类型"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.targetType)), new GUIContent("目标类型"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.isDynamicTarget)), new GUIContent("是否为动态目标"));
            if (_cmd.targetType != AnimTargetType.Dungeon)
            {
                if (_cmd.isDynamicTarget)
                {
                    NodeEditorGUILayout.DynamicPortList(nameof(_cmd.dynamicTargetList), typeof(ulong), serializedObject,
                        NodePort.IO.Input, onCreation: OnCreateList);
                }
                else
                {
                    NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.targetList)), new GUIContent("目标列表"));
                }
            }
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.animGraph)), new GUIContent("动画序列"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.onlyExecuteOnServer)), new GUIContent("是否仅在服务端执行"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.haveOtherInfo)), new GUIContent("是否有其他信息"));
            if (_cmd.haveOtherInfo)
            {
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_cmd.selectRes)), new GUIContent("选择资源"));
            }
            serializedObject.ApplyModifiedProperties();
        }
        
        private void OnCreateList(ReorderableList list)
        {
            list.drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "ID列表");
            };
            
            list.drawElementCallback = DrawElementCallback;
            return;
            
            void DrawElementCallback(Rect rect, int index, bool active, bool focused)
            {
                // var property = serializedObject.FindProperty(nameof(_cmd.dynamicTargetList)).GetArrayElementAtIndex(index);
                EditorGUI.LabelField(new Rect(rect.x, rect.y, 20, EditorGUIUtility.singleLineHeight), "ID");
                var port = _cmd.GetPort(nameof(_cmd.dynamicTargetList) + " " + index);
                var pos = rect.position + (port.IsOutput ? new Vector2(rect.width + 6, 0) : new Vector2(-36, 0));
                NodeEditorGUILayout.PortField(pos, port);
            }
        }
    }
}
