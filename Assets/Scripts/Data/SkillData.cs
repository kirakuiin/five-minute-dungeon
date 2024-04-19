using Data.Check;
using Data.Instruction;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "SkillData", menuName = "数据/技能数据", order = 0)]
    public class SkillData : ScriptableObject
    {
        [Tooltip("技能枚举")]
        public Skill skill;

        [Tooltip("技能名称")]
        public string skillName;

        [Tooltip("技能描述")]
        public string skillDesc;

        [Tooltip("技能图标")]
        public Sprite skillIcon;

        [Tooltip("行动")]
        public InstructionGraph action;

        [Tooltip("检查")]
        public CheckGraph check;
    }
}