using System;
using System.Collections;
using UnityEngine;

namespace UI.Common
{
    /// <summary>
    /// 让窗口向指定方向移动顶部。
    /// </summary>
    public class MoveAction : MonoBehaviour
    {
        [Tooltip("移动时间")]
        public float time = 0.5f;

        /// <summary>
        /// 让目标移动到指定位置。
        /// </summary>
        /// <param name="target"></param>
        /// <param name="destination"></param>
        /// <param name="onDone"></param>
        public void Move(Transform target, Vector3 destination, Action onDone=default)
        {
            StartCoroutine(MoveTo(target, destination, onDone));
        }

        private IEnumerator MoveTo(Transform tr, Vector3 pos, Action onDone)
        {
            float t = 0;
            var startPos = tr.position;
            while (true)
            {
                t += Time.deltaTime;
                float a = t / time;
                tr.position = Vector3.Lerp(startPos, pos, a);
                if (a >= 1.0f)
                    break;
                yield return null;
            }
            onDone?.Invoke();
        }
    }
}