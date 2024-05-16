using System.Collections;
using Data;
using Data.Animation;
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

        [SerializeField] private GameObject dizzy;

        private IPlayerRuntimeInfo _info;

        private bool isDizzy;
        
        public override void Init()
        {
            _info = GamePlayContext.Instance.GetPlayerRuntimeInfo(GetComponent<PlayerModel>().PlayerID);
            uiRoot.gameObject.SetActive(true);
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
            RefreshModelAnim();
        }

        private void RefreshHpBar()
        {
            var (draw, hand, discard, play) = (_info.GetDraws().Count, _info.GetHands().Count, _info.GetDiscards().Count, _info.PlayCardNum);
            hpBar.material.SetFloat(DrawNum, draw);
            hpBar.material.SetFloat(HandNum, hand);
            hpBar.material.SetFloat(DiscardNum, discard);
            hpBar.material.SetFloat(PlayNum, play);
        }
        
        private void RefreshModelAnim()
        {
            if (_info.GetHands().Count == 0 && _info.GetDraws().Count == 0)
            {
                StartCoroutine(ChangeModelCoroutine(AnimNameDefine.Dizzy));
            }
            else
            {
                if (isDizzy)
                {
                    StartCoroutine(ChangeModelCoroutine(AnimNameDefine.Idle));
                }
            }
        }

        private IEnumerator ChangeModelCoroutine(string stateName)
        {
            yield return new WaitForSeconds(0.4f);
            GetComponent<IModelAnimPlayer>().PlayAnim(stateName);
            isDizzy = stateName == AnimNameDefine.Dizzy;
            dizzy.SetActive(isDizzy);
        }

        private void LateUpdate()
        {
            uiRoot.transform.rotation = CameraControl.Instance.ActiveCamera.transform.rotation;
        }
    }
}