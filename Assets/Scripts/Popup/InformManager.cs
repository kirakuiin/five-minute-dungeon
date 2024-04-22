using GameLib.Common;
using UnityEngine;
using UnityEngine.Assertions;

namespace Popup
{
    public class InformManager : MonoSingleton<InformManager>
    {
        [SerializeField]
        private GameObject informPrefab;
        
        private Transform InformRoot { set; get; }

        protected override void OnInitialized()
        {
            var root = FindObjectOfType<InformRoot>();
            if (root != null)
            {
                InformRoot = root.transform;
            }
        }

        /// <summary>
        /// 创建一个通知弹窗。
        /// </summary>
        /// <param name="text">通知内容</param>
        /// <returns></returns>
        public GameObject CreateInform(string text)
        {
            var obj = Instantiate(informPrefab, InformRoot);
            obj.GetComponent<IInformDialog>().SetString(text);
            return obj;
        }

        private void OnValidate()
        {
            Assert.IsNotNull(informPrefab, "未设置预制体。");
            Assert.IsNotNull(informPrefab.GetComponent<IInformDialog>(),
                $"预制体未实现{nameof(IInformDialog)}接口。");
        }
    }
}