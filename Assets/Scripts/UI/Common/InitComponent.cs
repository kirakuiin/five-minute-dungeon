using UnityEngine;

namespace UI.Common
{
    /// <summary>
    /// 初始化组件
    /// </summary>
    public abstract class InitComponent: MonoBehaviour
    {
        /// <summary>
        /// 初始化。
        /// </summary>
        public abstract void Init();
    }
}