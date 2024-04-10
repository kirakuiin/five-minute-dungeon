using UnityEngine;

namespace Popup
{
    /// <summary>
    /// 可缓存弹窗对象。
    /// </summary>
    public class CacheablePopupBehaviour : MonoBehaviour, IPopupDialog
    {
        public bool IsCached { get; } = true;

        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Close()
        {
            gameObject.SetActive(false);
        }

        public GameObject Prefab  => gameObject;
    }
}