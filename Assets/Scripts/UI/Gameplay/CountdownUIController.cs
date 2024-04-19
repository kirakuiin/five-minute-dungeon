using System;
using Data.Check;
using Gameplay.Core;
using TMPro;
using UnityEngine;

namespace UI.Gameplay
{
    public class CountdownUIController : MonoBehaviour
    {
        [SerializeField] private TMP_Text timeText;

        private ITimeRuntimeInfo _timeInfo;

        /// <summary>
        /// 初始化。
        /// </summary>
        public void Init()
        {
            _timeInfo = GamePlayContext.Instance.GetTimeRuntimeInfo();
            InitConfig();
            SetTime(_timeInfo.RemainTime);
        }

        private void InitConfig()
        {
            _timeInfo.OnTimeUpdated += OnTimeUpdated;
        }

        private void OnTimeUpdated(int curTime)
        {
            SetTime(curTime);
        }

        private void SetTime(int curTime)
        {
            var timeSpan = new TimeSpan(0, 0, curTime);
            timeText.text = $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
        }
    }
}