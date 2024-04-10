using Popup;
using UnityEngine;

namespace UI.Menu
{
    /// <summary>
    /// 设置界面
    /// </summary>
    public class SettingController : MonoBehaviour
    {
        [SerializeField]
        private GameObject volumeDialog;

        public void OnButtonClick()
        {
            var myTransform = transform;
            PopupManager.Instance.CreatePopup(volumeDialog, myTransform.position, myTransform.parent);
        }
        
    }
}