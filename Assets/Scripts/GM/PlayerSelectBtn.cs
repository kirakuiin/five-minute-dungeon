using System;
using TMPro;
using UnityEngine;

namespace GM
{
    /// <summary>
    /// 玩家选择按钮。
    /// </summary>
    public class PlayerSelectBtn : MonoBehaviour
    {
        private Action _callback;

        [SerializeField] private TMP_Text text;
        
        public void Init(string desc, Action callback)
        {
            gameObject.SetActive(true);
            _callback = callback;
            text.text = desc;
        }

        public void OnClick(bool isSelected)
        {
            if (!isSelected) return;
            _callback?.Invoke();
        }
    }
}