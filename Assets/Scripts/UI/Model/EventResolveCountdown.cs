using System.Collections;
using Gameplay.Core;
using Gameplay.Core.State;
using UI.Common;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Model
{
    /// <summary>
    /// 事件结算倒计时。
    /// </summary>
    public class EventResolveCountdown : InitComponent
    {
        [SerializeField] private Slider slider;

        private bool _isAllowUpdate;

        private bool _isFlow = true;

        private Coroutine _coroutine;

        private void Start()
        {
            slider.minValue = 0;
            slider.maxValue = GameRule.EventCancelWaitTime;
        }

        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Init()
        {
            slider.gameObject.SetActive(false);
            InitListen();
        }

        private void InitListen()
        {
            GamePlayService.Instance.Status.OnStateChanged += OnStateChanged;
            GamePlayContext.Instance.GetTimeRuntimeInfo().OnTimeUpdated += OnTimeUpdated;
            GamePlayContext.Instance.GetTimeRuntimeInfo().OnTimeIsFlow += OnTimeIsFlow;
        }

        private void OnDisable()
        {
            GamePlayService.Instance.Status.OnStateChanged -= OnStateChanged;
            GamePlayContext.Instance.GetTimeRuntimeInfo().OnTimeUpdated -= OnTimeUpdated;
            GamePlayContext.Instance.GetTimeRuntimeInfo().OnTimeIsFlow -= OnTimeIsFlow;
        }

        private void OnStateChanged(GameServiceStatus status)
        {
            ResetCoroutine();
            UpdateVariable(status);
            if (!_isAllowUpdate)
            {
                slider.gameObject.SetActive(false);
            }
        }

        private void ResetCoroutine()
        {
            if (_coroutine == null) return;
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        private void UpdateVariable(GameServiceStatus status)
        {
            _isAllowUpdate = CheckStatus(status);
        }

        private bool CheckStatus(GameServiceStatus status)
        {
            return status is { state: ServiceState.EventResolve, stage: ServiceStage.Normal };
        }

        private void OnTimeUpdated(int time)
        {
            if (_isAllowUpdate)
            {
                if (_coroutine is null)
                {
                    slider.gameObject.SetActive(true);
                }
                _coroutine ??= StartCoroutine(UpdateSlider());
            }
        }

        private IEnumerator UpdateSlider()
        {
            slider.value = 0;
            var time = 0f;
            while (time < GameRule.EventCancelWaitTime)
            {
                if (_isFlow)
                {
                    time += Time.deltaTime;
                    slider.value = time;
                }
                yield return null;
            }
        }

        private void OnTimeIsFlow(bool isFlow)
        {
            _isFlow = isFlow;
        }
    }
}