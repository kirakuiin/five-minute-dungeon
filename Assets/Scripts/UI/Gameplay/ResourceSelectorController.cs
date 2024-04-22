using Data;
using Data.Check;
using Gameplay.Core;
using Popup;
using UnityEngine;

namespace UI.Gameplay
{
    /// <summary>
    /// 资源选择控制器。
    /// </summary>
    public class ResourceSelectorController : MonoBehaviour
    {
        [SerializeField] private GameObject uiPrefab;
        
        private IResourceSelector Selector { set; get; }
        
        /// <summary>
        /// 初始化。
        /// </summary>
        public void Init()
        {
            Selector = GamePlayContext.Instance.GetPlayerRuntimeInfo().GetRuntimeInteractive().GetResourceSelector();
            Selector.OnResourceSelecting += OnResourceSelecting;
        }

        private void OnResourceSelecting()
        {
            PopupManager.Instance.CreatePopup(uiPrefab, OnCreateDone);
        }

        private void OnCreateDone(GameObject obj)
        {
            obj.GetComponent<ResourceChoiceUIController>().Init(this);
        }

        /// <summary>
        /// 设置选择的资源。
        /// </summary>
        /// <param name="type"></param>
        public void SetSelectResource(Resource type)
        {
            Selector.SelectResource(type);
        }
    }
}