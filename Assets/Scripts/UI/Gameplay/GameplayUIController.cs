using Data;
using Data.Check;
using GameLib.Common;
using GameLib.Network.NGO.ConnectionManagement;
using Gameplay.Core;
using Gameplay.GameState;
using Gameplay.Message;
using Popup;
using TMPro;
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

        [SerializeField] private ClassSkillUIController skillUI;

        [SerializeField] private HandZoneUIController handUI;

        [SerializeField] private LevelUIController levelUI;

        [SerializeField] private ResourceSelectorController resSelectUI;

        [SerializeField] private EnemySelectorController enemySelectUI;

        private IPlayerRuntimeInfo RuntimeInfo => GamePlayContext.Instance.GetPlayerRuntimeInfo();

        private readonly DisposableGroup _disposable = new();
        
        private void Start()
        {
            LockUI();
            InitListen();
        }
        
        private void InitListen()
        {
            _disposable.Add(state.GameplayState.Subscribe(OnGamePlayStateChanged));
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
            skillUI.Init();
            handUI.Init();
            levelUI.Init();
            resSelectUI.Init();
            enemySelectUI.Init();
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
            var controller = dialog.GetComponent<CardPileUIController>();
            controller.Init(RuntimeInfo.GetDraws(), ShowType.Statistics);
        }

        public void OnShowDiscardPile()
        {
            PopupManager.Instance.CreatePopup(discardUIPrefab, OnCreateDiscardDone);
        }

        private void OnCreateDiscardDone(GameObject dialog)
        {
            var controller = dialog.GetComponent<CardPileUIController>();
            controller.Init(RuntimeInfo.GetDiscards(), ShowType.ByOrder);
        }
    }
}