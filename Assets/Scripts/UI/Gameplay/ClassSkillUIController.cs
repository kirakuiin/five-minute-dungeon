using Data;
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

        [SerializeField] private Image skillIconUI;
        
        public void Init(IPlayerRuntimeInfo info)
        {
            _playerInfo = info;
            InitUI();
        }

        private void InitUI()
        {
            var classData = DataService.Instance.GetClassData(_playerInfo.PlayerClass);
            skillIconUI.sprite = classData.skillData.skillIcon;
            skillIconUI.color = classData.classColor;
        }

        public void CastSkill()
        {
        }
    }
}