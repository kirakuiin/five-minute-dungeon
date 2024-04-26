using System.Collections.Generic;
using Data.Instruction;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "ChallengeCardData", menuName = "数据/卡牌数据/挑战卡", order = 2)]
    public class ChallengeCardData: EnemyScriptObj
    {
        [Tooltip("卡牌枚举")]
        public Challenge card;

        [Tooltip("卡牌类型")]
        public EnemyCardType enemyCardType;

        [Tooltip("行动")]
        public InstructionGraph action;
    }
}