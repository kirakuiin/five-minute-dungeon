﻿using GameLib.Common;
using UnityEngine;
using UnityEngine.Assertions;

namespace Popup
{
    /// <summary>
    /// 锁定屏幕，防止操作。
    /// </summary>
    public class LockScreenManager : MonoSingleton<LockScreenManager>
    {
        [SerializeField]
        private GameObject lockPrefab;

        private GameObject _lockDialog;
        
        private Transform LockRoot { set; get; }

        protected override void OnInitialized()
        {
            var root = FindObjectOfType<LockRoot>();
            if (root != null)
            {
                LockRoot = root.transform;
            }
        }
        
        /// <summary>
        /// 创建一个锁定窗口。
        /// </summary>
        /// <param name="text">锁定内容</param>
        /// <returns></returns>
        public void Lock(string text)
        {
            _lockDialog ??= Instantiate(lockPrefab, LockRoot);
            _lockDialog.GetComponent<ILockDialog>().SetString(text);
        }

        /// <summary>
        /// 解锁屏幕。
        /// </summary>
        public void Unlock()
        {
            if (_lockDialog is null) return;
            _lockDialog.SetActive(false);
        }

        private void OnValidate()
        {
            Assert.IsNotNull(lockPrefab, "未设置预制体。");
            Assert.IsNotNull(lockPrefab.GetComponent<ILockDialog>(),
                $"预制体未实现{nameof(ILockDialog)}接口。");
        }
    }
}