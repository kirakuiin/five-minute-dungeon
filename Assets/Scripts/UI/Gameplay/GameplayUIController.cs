using System;
using GameLib.Common;
using GameLib.Network.NGO.ConnectionManagement;
using Gameplay.Core;
using Gameplay.Data;
using Gameplay.GameState;
using Gameplay.Message;
using Popup;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace UI.Gameplay
{
    /// <summary>
    /// 玩法ui控制器。
    /// </summary>
    public class GameplayUIController : MonoBehaviour
    {
        [SerializeField] private GamePlayState state;

        [SerializeField] private TMP_Text drawNum;

        [SerializeField] private TMP_Text discardNum;

        [SerializeField] private GameObject drawUIPrefab;

        [SerializeField] private GameObject discardUIPrefab;

        [SerializeField] private Transform uiParent;

        [SerializeField] private ClassSkillUIController skillUI;

        [SerializeField] private HandZoneUIController handUI;

        [SerializeField] private LevelUIController levelUI;

        private readonly DisposableGroup _disposableGroup = new();
        
        private ulong PlayerID => NetworkManager.Singleton.LocalClientId;

        private IPlayerRuntimeInfo RuntimeInfo => GamePlayContext.Instance.GetPlayerRuntimeInfo(PlayerID);
        
        private void Start()
        {
            LockUI();
            InitListen();
        }
        
        private void InitListen()
        {
            _disposableGroup.Add(state.GameplayState.Subscribe(OnGamePlayStateChanged));
        }

        private void LockUI()
        {
            if (state.current.state == GamePlayStateEnum.NotStart)
            {
                LockScreenManager.Instance.Lock("等待玩家数据同步。");
            }
        }

        private void OnGamePlayStateChanged(GamePlayStateMsg msg)
        {
            if (msg.state == GamePlayStateEnum.InitDone)
            {
                LockScreenManager.Instance.Unlock();
                InitUI();
            }
        }

        private void InitUI()
        {
            RuntimeInfo.GetDraws().OnCardChanged += OnDrawChanged;
            RuntimeInfo.GetDiscards().OnCardChanged += OnDiscardChanged;
            SetDrawPileText();
            SetDiscardPileText();
            skillUI.Init(RuntimeInfo);
            handUI.Init(RuntimeInfo);
            levelUI.Init();
        }

        private void SetDrawPileText()
        {
            drawNum.text = $"{RuntimeInfo.GetDraws().Count}";
        }

        private void SetDiscardPileText()
        {
            discardNum.text = $"{RuntimeInfo.GetDiscards().Count}";
        }

        private void OnDrawChanged(CardChangeEvent e)
        {
            SetDrawPileText();
        }

        private void OnDiscardChanged(CardChangeEvent e)
        {
            SetDiscardPileText();
        }

        public void OnExit()
        {
            ConnectionManager.Instance.UserRequestShutdown();
        }

        public void OnShowDrawPile()
        {
            PopupManager.Instance.CreatePopup(drawUIPrefab, OnCreateDrawDone);
        }

        private void OnCreateDrawDone(GameObject dialog)
        {
            dialog.transform.SetParent(uiParent, false);
            var controller = dialog.GetComponent<CardPileUIController>();
            controller.Init(RuntimeInfo.GetDraws(), ShowType.Statistics);
        }

        public void OnShowDiscardPile()
        {
            PopupManager.Instance.CreatePopup(discardUIPrefab, OnCreateDiscardDone);
        }

        private void OnCreateDiscardDone(GameObject dialog)
        {
            dialog.transform.SetParent(uiParent, false);
            var controller = dialog.GetComponent<CardPileUIController>();
            controller.Init(RuntimeInfo.GetDiscards(), ShowType.ByOrder);
        }
    }
}