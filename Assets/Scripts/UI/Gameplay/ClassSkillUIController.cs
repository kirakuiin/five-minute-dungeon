using Data;
using Data.Check;
using Gameplay.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Gameplay
{
    /// <summary>
    /// 玩家技能控件。
    /// </summary>
    public class ClassSkillUIController : MonoBehaviour
    {
        private IPlayerRuntimeInfo _playerInfo;

        private ClassData _classData;

        [SerializeField] private Image skillIconUI;
        
        public void Init(IPlayerRuntimeInfo info)
        {
            _playerInfo = info;
            _classData = DataService.Instance.GetClassData(_playerInfo.PlayerClass);
            InitUI();
        }

        private void InitUI()
        {
            skillIconUI.sprite = _classData.skillData.skillIcon;
            skillIconUI.color = _classData.classColor;
        }

        public void CastSkill()
        {
            GamePlayService.Instance.CastSkill(_classData.skillData.skill);
        }
    }
}