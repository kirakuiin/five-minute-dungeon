using System.Collections.Generic;
using Data.Check;
using Data.Instruction;
using UnityEngine;

namespace Data 
{
    /// <summary>
    /// 配置玩家卡牌数据。
    /// </summary>
    [CreateAssetMenu(fileName = "CardData", menuName = "数据/卡牌数据/玩家卡牌", order = 0)]
    public class CardData: ScriptableObject
    {
        [Tooltip("卡牌枚举")]
        public Card card;

        [Tooltip("卡牌类型")]
        public PlayerCardType playerCardType;
        
        [Tooltip("正面卡图")]
        public Sprite cardFront;
        
        [Tooltip("文字距离中心偏移量")]
        public float textOffset;

        [Tooltip("卡牌名称")]
        public string cardName;

        [Tooltip("卡牌描述")]
        public string cardDescription;

        [Tooltip("执行动作")]
        public InstructionGraph action;

        [Tooltip("执行检查")]
        public CheckGraph check;
    }
}
