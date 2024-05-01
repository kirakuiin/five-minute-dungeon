using System;
using System.Collections.Generic;
using Data;
using GameLib.Common.Extension;
using Gameplay.Data;
using Gameplay.GameState;
using Gameplay.Progress;
using Popup;
using TMPro;
using UI.Model;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace UI.PostGame
{
    public class PostGameUIController : NetworkBehaviour
    {
        [SerializeField] private PostGameState state;
        
        [SerializeField] private List<ObjConfig> showList;

        [SerializeField] private List<GameObject> completeList;

        [SerializeField] private Slider progress;

        [SerializeField] private TMP_Text useTimeText;

        [SerializeField] private TMP_Text bossName;

        [SerializeField] private List<Transform> playerPosList;

        [SerializeField] private Transform BossPos;
        
        private static ChallengeResultData Result => GameProgress.Instance.ChallengeResult;

        private void Awake()
        {
            LockScreenManager.Instance.Lock("等待结算中...");
        }

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                SyncProgressDataRpc(Result);
            }
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void SyncProgressDataRpc(ChallengeResultData data)
        {
            GameProgress.Instance.UpdateResult(data);
            InitUI();
            LockScreenManager.Instance.Unlock();
        }

        private void InitUI()
        {
            InitCommonUI();
            InitModel();
            if (!GameProgress.Instance.HasNextBoss() && Result.isWin)
            {
                InitCompleteUI();
            }
            else
            {
                InitNormalUI();
            }
        }

        private void InitCommonUI()
        {
            progress.value = GameProgress.Instance.BossPercent;
            var timeSpan = new TimeSpan(0, 0, Result.useTime);
            useTimeText.text = $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
            bossName.text = $"{DataService.Instance.GetBossData(GameProgress.Instance.CurrentBoss).desc}";
            bossName.gameObject.transform.position += new Vector3(
                progress.gameObject.GetComponent<RectTransform>().rect.width * GameProgress.Instance.BossPercent, 0);
        }

        private void InitModel()
        {
            InitPlayerModel();
            InitEnemyModel();
        }

        private void InitPlayerModel()
        {
            var i = 0;
            foreach (var playerClass in Result.squadList)
            {
                var prefab = DataService.Instance.GetClassData(playerClass).classPrefab;
                var obj = Instantiate(prefab, playerPosList[i]);
                i += 1;
                obj.GetComponent<PlayerModel>().InitAsPureModel();
                if (Result.isWin)
                {
                    obj.GetComponent<ModelAnimController>().PlayWin();
                }
                else
                {
                    obj.GetComponent<ModelAnimController>().PlayLose();
                }
            }
        }

        private void InitEnemyModel()
        {
            var prefab = DataService.Instance.GetBossData(GameProgress.Instance.CurrentBoss).prefab;
            var obj = Instantiate(prefab, BossPos);
            obj.GetComponent<EnemyModel>().InitAsPureModel();
            if (Result.isWin)
            {
                obj.GetComponent<ModelAnimController>().PlayLose();
            }
            else
            {
                obj.GetComponent<ModelAnimController>().PlayWin();
            }
        }

        private void InitCompleteUI()
        {
            completeList.Apply(obj => obj.SetActive(true));
        }

        private void InitNormalUI()
        {
            foreach (var config in showList)
            {
                if (IsServer)
                {
                    SetServerUI(config);
                }
                else
                {
                    SetClientUI(config);
                }
            }
        }

        private void SetServerUI(ObjConfig config)
        {
            if (config.isShowOnServer)
            {
                SetByResult(config);
            }
        }

        private void SetByResult(ObjConfig config)
        {
            if ((config.isShowOnWin && Result.isWin)
                || (config.isShowOnLose && !Result.isWin))
            {
                config.obj.SetActive(true);
            }
            else
            {
                config.obj.SetActive(false);
            }
        }
        
        private void SetClientUI(ObjConfig config)
        {
            if (config.isShowOnClient)
            {
                SetByResult(config);
            }
        }

        public void GoBackToLobby()
        {
            if (Result.isWin)
            {
                GameProgress.Instance.ChallengeNextBoss();
            }
            state.GoBackToLobby();
        }

        public void ReturnToMain()
        {
            GameProgress.Instance.Reset();
            state.GoBackToMain();
        }

        public void Retry()
        {
            state.GoBackToGamePlay();
        }
        
        public void NextChallenge()
        {
            GameProgress.Instance.ChallengeNextBoss();
            state.GoBackToGamePlay();
        }
    }

    [Serializable]
    public struct ObjConfig
    {
        public GameObject obj;
        public bool isShowOnServer;
        public bool isShowOnClient;
        public bool isShowOnWin;
        public bool isShowOnLose;
    }
}