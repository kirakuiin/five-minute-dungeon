using System.Text;
using Data;
using Data.Check;
using Gameplay.Core;
using TMPro;
using Unity.Netcode;
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

        [SerializeField] private CountdownUIController countdown;

        [SerializeField] private GameObject continueBtn;

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
            countdown.Init();
            RefreshUI();
        }

        private void InitConfig()
        {
            progress.maxValue = _levelInfo.TotalLevelNum;
            progress.minValue = 0;
            _levelInfo.OnEnemyAdded += OnEnemyAdded;
            _levelInfo.OnEnemyDestroyed += OnEnemyDestroyed;
            if (NetworkManager.Singleton.IsServer)
            {
                GamePlayContext.Instance.GetTimeRuntimeInfo().OnTimeIsFlow += OnTimeIsFlow;
            }
        }

        private void OnTimeIsFlow(bool isFlow)
        {
            continueBtn.SetActive(!isFlow);
        }

        private void OnEnemyAdded(EnemyChangeEvent e)
        {
            RefreshUI();
        }
        
        private void OnEnemyDestroyed(EnemyChangeEvent e)
        {
            RefreshUI();
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
            foreach (var enemy in _levelInfo.GetAllEnemiesInfo().Values)
            {
                if (!enemy.IsEventCard()) continue;
                var data = DataService.Instance.GetEnemyCardData(enemy);
                if (data is ChallengeCardData e)
                {
                    content.Append($"结算效果: {e.resolveText}\n");
                }
            }

            var str = content.ToString();
            enemyDesc.text = str.Length > 0 ? str.Substring(0, str.Length - 1) : "";
        }
        
        public void ContinueTime()
        {
            GamePlayContext.Instance.GetTimeController().Continue();
        }
    }
}