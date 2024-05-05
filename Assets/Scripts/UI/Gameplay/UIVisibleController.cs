using System.Collections.Generic;
using GameLib.Common.Extension;
using UnityEngine;

namespace UI.Gameplay
{
    /// <summary>
    /// 控制UI显示
    /// </summary>
    public class UIVisibleController : MonoBehaviour
    {
        [SerializeField] private List<GameObject> uiObjs;

        [SerializeField] private List<MonoBehaviour> uiComps;

        /// <summary>
        /// 设置可见性。
        /// </summary>
        /// <param name="isVisible"></param>
        public void SetVisible(bool isVisible)
        {
            uiComps.Apply(obj => obj.enabled = isVisible);
            uiObjs.Apply(obj => obj.SetActive(isVisible));
        }
    }
}