using Data;
using Data.Check;
using Gameplay.Camera;
using Gameplay.Core;
using TMPro;
using UI.Common;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Model
{
    /// <summary>
    /// 玩家模型UI
    /// </summary>
    public class PlayerInitUIController : InitComponent
    {
        private static readonly int DrawNum = Shader.PropertyToID("_DrawNum");
        private static readonly int HandNum = Shader.PropertyToID("_HandNum");
        private static readonly int DiscardNum = Shader.PropertyToID("_DiscardNum");
        private static readonly int PlayNum = Shader.PropertyToID("_PlayNum");

        [SerializeField] private Transform uiRoot;

        [SerializeField] private TMP_Text playerName;

        [SerializeField] private Image hpBar;

        private IPlayerRuntimeInfo _info;
        
        public override void Init()
        {
            _info = GamePlayContext.Instance.GetPlayerRuntimeInfo(GetComponent<PlayerModel>().PlayerID);
            InitUI();
            InitListen();
            RefreshHpBar();
        }

        private void InitUI()
        {
            hpBar.material = Instantiate(hpBar.material);
            playerName.text = _info.PlayerName;
        }

        private void InitListen()
        {
            _info.GetHands().OnCardChanged += OnCardChanged;
        }

        private void OnCardChanged(CardChangeEvent @event)
        {
            RefreshHpBar();
        }

        private void RefreshHpBar()
        {
            var (draw, hand, discard, play) = (_info.GetDraws().Count, _info.GetHands().Count, _info.GetDiscards().Count, _info.PlayCardNum);
            hpBar.material.SetFloat(DrawNum, draw);
            hpBar.material.SetFloat(HandNum, hand);
            hpBar.material.SetFloat(DiscardNum, discard);
            hpBar.material.SetFloat(PlayNum, play);
        }

        private void LateUpdate()
        {
            uiRoot.transform.rotation = CameraControl.Instance.ActiveCamera.transform.rotation;
        }
    }
}