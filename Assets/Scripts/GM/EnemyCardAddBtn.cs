using System;
using Data;
using TMPro;
using UnityEngine;

namespace GM
{
    /// <summary>
    /// 敌方卡牌按钮。
    /// </summary>
    public class EnemyCardAddBtn : MonoBehaviour
    {
        private Action _callback;

        [SerializeField] private TMP_Text text;
        
        public void Init(string desc, Action callback)
        {
            _callback = callback;
            text.text = desc;
        }

        public void OnClick()
        {
            _callback?.Invoke();
        }
    }
}