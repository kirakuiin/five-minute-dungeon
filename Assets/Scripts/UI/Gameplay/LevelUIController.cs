﻿using System;
using System.Text;
using Data;
using Gameplay.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Gameplay
{
    /// <summary>
    /// 关卡信息控制器。
    /// </summary>
    public class LevelUIController : MonoBehaviour
    {
        [SerializeField] private TMP_Text enemyDesc;
        
        [SerializeField] private Slider progress;

        [SerializeField] private NeedResourcePoolUIController needZone;
        
        [SerializeField] private PlayedResourcePoolUIController playedZone;

        private ILevelRuntimeInfo _levelInfo;

        /// <summary>
        /// 初始化。
        /// </summary>
        public void Init()
        {
            _levelInfo = GamePlayContext.Instance.GetLevelRuntimeInfo();
            InitConfig();
            needZone.Init();
            playedZone.Init();
            RefreshUI();
        }

        private void InitConfig()
        {
            progress.maxValue = _levelInfo.TotalLevelNum;
            progress.minValue = 0;
            _levelInfo.OnEnemyAdded += OnEnemyAdded;
        }

        private void OnEnemyAdded(EnemyChangeEvent e)
        {
            RefreshUI();
        }

        private void OnDestroy()
        {
            if (_levelInfo != null)
            {
                _levelInfo.OnEnemyAdded -= OnEnemyAdded;
            }
        }

        private void RefreshUI()
        {
            RefreshProgress();
            RefreshDesc();
            needZone.RefreshUI();
            playedZone.RefreshUI();
        }

        private void RefreshProgress()
        {
            progress.value = _levelInfo.CurProgress;
        }

        private void RefreshDesc()
        {
            StringBuilder content = new();
            foreach (var enemy in _levelInfo.GetAllEnemyInfos().Values)
            {
                var data = DataService.Instance.GetEnemyCardData(enemy);
                content.Append($"{data.desc}\n");
            }

            var str = content.ToString();
            enemyDesc.text = str.Substring(0, str.Length - 1);
        }
    }
}