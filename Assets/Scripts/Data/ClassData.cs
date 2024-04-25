using UnityEngine;

namespace Data
{
    /// <summary>
    /// 职业数据。
    /// </summary>
    [CreateAssetMenu(fileName = "ClassData", menuName = "数据/职业数据", order = 0)]
    public class ClassData : ScriptableObject
    {
        [Tooltip("职业类型")]
        public Class classType;

        [Tooltip("职业名称")]
        public string className;

        [Tooltip("技能数据")]
        public SkillData skillData;

        [Tooltip("牌组数据")]
        public DeckData deckData;

        [Tooltip("头像")]
        public Sprite avatar;

        [Tooltip("肖像")]
        public Sprite portraits;

        [Tooltip("职业颜色")]
        public Color classColor;

        [Tooltip("职业模型")]
        public GameObject classPrefab;
    }
}