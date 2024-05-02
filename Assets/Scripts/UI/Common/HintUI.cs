using System.Collections.Generic;
using System.Drawing;
using Popup;
using TMPro;
using UnityEngine;

namespace UI.Common
{
    /// <summary>
    /// 文本提示框。
    /// </summary>
    public class HintUI : CacheablePopupBehaviour
    {
        [SerializeField] private TMP_Text descText;

        private static readonly Vector2 LeftBottom = new Vector2(0, 0);
        private static readonly Vector2 LeftTop = new Vector2(0, 1);
        private static readonly Vector2 RightBottom = new Vector2(1, 0);
        private static readonly Vector2 RightTop = new Vector2(1, 1);
        
        private static readonly List<Vector2> PivotList = new(){LeftTop, RightBottom, LeftBottom, RightTop};

        /// <summary>
        /// 自动选定锚点，确保提示位于屏幕内。
        /// </summary>
        /// <param name="text"></param>
        /// <param name="pos"></param>
        public void ShowInScreen(string text, Vector3 pos)
        {
            var ui = GetComponent<HintUI>();
            ui.Init(text);
            foreach (var pivot in PivotList)
            {
                ui.SetPos(pos, pivot);
                if (IsInScreen()) break;
            }
            gameObject.SetActive(true);
        }

        private bool IsInScreen()
        {
            var corners = new Vector3[4];
            GetComponent<RectTransform>().GetWorldCorners(corners);
            var (origin, topRight) = (corners[0], corners[2]);
            var screenOrigin = RectTransformUtility.WorldToScreenPoint(null, origin);
            var uiRect = new Rectangle((int)screenOrigin.x, (int)screenOrigin.y,
                (int)(topRight.x-origin.x), (int)(topRight.y-origin.y));
            var screenRect = new Rectangle(0, 0, Screen.width, Screen.height);
            return screenRect.Contains(uiRect);
        }

        /// <summary>
        /// 以左上角为锚点初始化提示窗。
        /// </summary>
        /// <param name="text"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public void ShowAtLeftTop(string text, Vector3 pos)
        {
            Create(text, pos, LeftTop);
        }

        private void Create(string text, Vector3 pos, Vector2 mode)
        {
            GetComponent<HintUI>().Init(text);
            GetComponent<HintUI>().SetPos(pos, mode);
            gameObject.SetActive(true);
        }

        private void Init(string text)
        {
            descText.text = text;
        }

        private void SetPos(Vector3 pos, Vector2 alignMode)
        {
            GetComponent<RectTransform>().pivot = alignMode;
            transform.position = pos;
        }

        public override void Show()
        {
        }
    }
}