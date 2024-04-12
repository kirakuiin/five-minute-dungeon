﻿using System;
using Data.Instruction;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;

namespace Gameplay.Core
{
    /// <summary>
    /// 时间控制器。
    /// </summary>
    public class TimeController : NetworkBehaviour, ITimeController, ITimeRuntimeInfo
    {
        private readonly NetworkVariable<int> _timeValue = new NetworkVariable<int>();

        private float _curRemainTime;

        private bool _isRunning;

        public override void OnNetworkSpawn()
        {
            if (IsClient)
            {
                _timeValue.OnValueChanged += OnValueChanged;
            }
        }

        public override void OnNetworkDespawn()
        {
            if (IsClient)
            {
                _timeValue.OnValueChanged -= OnValueChanged;
            }
        }

        private void OnValueChanged(int oldVal, int newVal)
        {
            OnTimeUpdated?.Invoke(newVal);
            if (newVal == 0)
            {
                OnTimeout?.Invoke();
            }
        }

        public void StartTimer(int totalCountdown)
        {
            _curRemainTime = totalCountdown;
            _timeValue.Value = totalCountdown;
            _isRunning = true;
        }

        public void Stop()
        {
            _isRunning = false;
        }

        public void Continue()
        {
            _isRunning = true;
        }

        public int RemainTime => _timeValue.Value;
        
        public event Action<int> OnTimeUpdated;
        
        public event Action OnTimeout;

        private void Update()
        {
            if (!IsServer || !_isRunning || _curRemainTime <= 0) return;
            _curRemainTime -= Time.deltaTime;
            int realRemain = (int)math.floor(_curRemainTime) + 1;
            if (realRemain >= 0 && realRemain != _timeValue.Value)
            {
                _timeValue.Value = realRemain;
            }
        }
    }
}