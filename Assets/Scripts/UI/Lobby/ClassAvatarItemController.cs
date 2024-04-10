using System;
using Data;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Lobby
{
    /// <summary>
    /// 职业头像选择栏
    /// </summary>
    public class ClassAvatarItemController : MonoBehaviour
    {
        [SerializeField] private Toggle toggle;

        [SerializeField] private Image selectImg;

        [SerializeField] private Image notSelectImg;
        
        private Action<ClassData> _action;

        private ClassData _data;

        /// <summary>
        /// 初始化头像栏。
        /// </summary>
        /// <param name="data"></param>
        /// <param name="group"></param>
        /// <param name="onToggle"></param>
        public void Init(ClassData data, ToggleGroup group, Action<ClassData> onToggle)
        {
            toggle.group = group;
            _data = data;
            selectImg.sprite = data.avatar;
            notSelectImg.sprite = data.avatar;
            _action = onToggle;
        }

        public void OnValueChanged(bool isSelected)
        {
            if (isSelected)
            {
                _action?.Invoke(_data);
            }
        }
    }
}