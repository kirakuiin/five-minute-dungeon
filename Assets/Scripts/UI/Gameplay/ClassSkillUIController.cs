﻿using Data;
using Data.Check;
using GameLib.Common.Extension;
using Gameplay.Core;
using Gameplay.Core.State;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Gameplay
{
    /// <summary>
    /// 玩家技能控件。
    /// </summary>
    public class ClassSkillUIController : MonoBehaviour
    {
        private IPlayerRuntimeInfo RuntimeInfo => GamePlayContext.Instance.GetPlayerRuntimeInfo();

        private ClassData _classData;

        private Skill _skill;

        private GamePlayService Service => GamePlayService.Instance;

        [SerializeField] private Image skillIconUI;

        [SerializeField] private Button castBtn;

        [SerializeField] private HintUIController hint;

        [SerializeField] private Button cancelBtn;

        private SkillState _state = SkillState.Done;
        
        public void Init()
        {
            _classData = DataService.Instance.GetClassData(RuntimeInfo.PlayerClass);
            _skill = _classData.skillData.skill;
            InitUI();
            InitListen();
        }

        private void InitListen()
        {
            RuntimeInfo.GetHands().OnCardChanged += OnCardChanged;
            GamePlayService.Instance.Status.OnStateChanged += OnStateChanged;
            GamePlayService.Instance.Status.OnSkillStateChanged += OnSkillStateChanged;
        }

        private void OnSkillStateChanged(ulong clientID, SkillState state)
        {
            if (clientID != NetworkManager.Singleton.LocalClientId) return;
            _state = state;
            UpdateSkillBtnState();
        }

        private void OnStateChanged(GameServiceStatus status)
        {
            UpdateSkillBtnState();
        }

        private void OnCardChanged(CardChangeEvent e)
        {
            UpdateSkillBtnState();
        }

        private void UpdateSkillBtnState()
        {
            var isResolve = _state is SkillState.Resolve;
            
            SetCastBtnVisible(!isResolve);
            SetCancelBtnVisible(isResolve);
            
            var result = Service.Status.CanICastSkill(_skill);
            result = result && _state is SkillState.Done;
            SetCastBtnState(result);
        }

        private void SetCastBtnVisible(bool isVisible)
        {
            castBtn.GetComponent<UIVisibleController>().SetVisible(isVisible);
        }

        private void SetCancelBtnVisible(bool isVisible)
        {
            cancelBtn.GetComponent<UIVisibleController>().SetVisible(isVisible);
        }

        private void SetCastBtnState(bool isShow)
        {
            castBtn.interactable = isShow;
            skillIconUI.color = isShow ? _classData.classColor : Color.grey;
        }

        private void InitUI()
        {
            skillIconUI.sprite = _classData.skillData.skillIcon;
            skillIconUI.color = _classData.classColor;
            hint.hintText = $"{_classData.skillData.skillDesc.ToRichText(_classData.classColor)}";
        }

        public void CastSkill()
        {
            Service.CastSkill(_skill);
            _state = SkillState.Waiting;
            UpdateSkillBtnState();
        }

        public void CancelSkill()
        {
            RuntimeInfo.GetRuntimeInteractive().GetHandSelector().CancelSelectHand();
            RuntimeInfo.GetRuntimeInteractive().GetEnemySelector().CancelSelectEnemy();
        }
    }
}