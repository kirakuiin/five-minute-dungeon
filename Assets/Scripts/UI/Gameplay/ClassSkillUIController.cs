﻿using Data;
using Data.Check;
using Gameplay.Core;
using Gameplay.Core.State;
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
            GamePlayService.Instance.OnStateChanged += OnStateChanged;
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
            castBtn.interactable = Service.CanICastSkill(_skill);
        }

        private void InitUI()
        {
            skillIconUI.sprite = _classData.skillData.skillIcon;
            skillIconUI.color = _classData.classColor;
        }

        public void CastSkill()
        {
            Service.CastSkill(_skill);
        }
    }
}