using System;
using System.Collections.Generic;
using GameLib.Common;
using UnityEngine;
using UnityEngine.Assertions;

namespace Popup
{
    /// <summary>
    /// 弹窗管理器，用来处理一般弹窗的创建流程。
    /// </summary>
    public class PopupManager : MonoSingleton<PopupManager>
    {
        private readonly Dictionary<IPopupDialog, GameObject> _cache = new();
        
        /// <summary>
        /// 页面父节点。
        /// </summary>
        public Transform DialogRoot { set; get; }

        protected override void OnInitialized()
        {
            var root = FindObjectOfType<DialogRoot>();
            if (root != null)
            {
                DialogRoot = root.transform;
            }
        }

        /// <summary>
        /// 创建弹窗。
        /// </summary>
        /// <param name="dialogPrefab">弹窗预制体</param>
        /// <param name="onCreateDone">创建成功回调</param>
        /// <returns>创建的游戏对象</returns>
        public IPopupDialog CreatePopup(GameObject dialogPrefab, Action<GameObject> onCreateDone=null)
        {
            var component = dialogPrefab.GetComponent<IPopupDialog>();
            Assert.IsNotNull(component, $"弹窗预制体未添加{nameof(IPopupDialog)}组件。");

            var isFirstCreate = !_cache.ContainsKey(component);
            var popup = GetPopup(component);
            popup.transform.SetParent(DialogRoot, false);
            
            if (isFirstCreate)
            {
                onCreateDone?.Invoke(popup);
            }
            var dialog = popup.GetComponent<IPopupDialog>();
            dialog.Show();
            return dialog;
        }

        /// <summary>
        /// 以指定的父节点和位置创建弹窗。
        /// </summary>
        /// <param name="dialogPrefab">预制体</param>
        /// <param name="position">世界坐标</param>
        /// <param name="parent">父变换</param>
        /// <param name="onCreateDone">创建成功回调</param>
        /// <returns>窗口对象</returns>
        public IPopupDialog CreatePopup(GameObject dialogPrefab, Vector3 position, Transform parent,
            Action<GameObject> onCreateDone=null)
        {
            var dialog = CreatePopup(dialogPrefab, onCreateDone);
            var dialogTransform = dialog.Prefab.transform;
            dialogTransform.SetParent(parent);
            dialogTransform.position = position;
            return dialog;
        }

        private GameObject GetPopup(IPopupDialog dialog)
        {
            if (dialog.IsCached && _cache.TryGetValue(dialog, out GameObject obj))
            {
                if (obj != null && obj.GetComponent<IPopupDialog>() != null)
                {
                    return obj;
                }
            }
            var instance = Instantiate(dialog.Prefab);
            if (dialog.IsCached)
            {
                _cache[dialog] = instance;
            }
            return instance;
        }
    }
}