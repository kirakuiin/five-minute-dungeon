using UnityEngine;

namespace Popup
{
    public interface IPopupDialog
    {
        /// <summary>
        /// 显示窗口。
        /// </summary>
        public void Show();

        /// <summary>
        /// 关闭窗口。
        /// </summary>
        public void Close();

        /// <summary>
        /// 是否进行缓存。
        /// </summary>
        public bool IsCached { get; }
        
        /// <summary>
        /// 获得UI预制体。
        /// </summary>
        public GameObject Prefab{ get; }
    }
    
    /// <summary>
    /// 通知窗口接口
    /// </summary>
    public interface IInformDialog
    {
        /// <summary>
        /// 设置通知文本。
        /// </summary>
        /// <param name="text"></param>
        public void SetString(string text);
    }
    
    /// <summary>
    /// 锁定窗口接口
    /// </summary>
    public interface ILockDialog
    {
        /// <summary>
        /// 设置锁定文本。
        /// </summary>
        /// <param name="text"></param>
        public void SetString(string text);
    }
}