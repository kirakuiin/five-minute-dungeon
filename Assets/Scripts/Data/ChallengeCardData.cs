using System.Collections.Generic;
using Data.Instruction;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "ChallengeCardData", menuName = "数据/卡牌数据/挑战卡", order = 2)]
    public class ChallengeCardData: DictionaryScriptObj<Resource, int>, IEnemyCard
    {
        [Tooltip("卡牌枚举")]
        public Challenge card;

        [Tooltip("卡牌类型")]
        public EnemyCardType enemyCardType;

        [Tooltip("行动")]
        public InstructionGraph action;
        
        public EnemyCardType Type => enemyCardType;

        public string desc;

        public int CardValue => (int)card;

        public IEnumerable<Resource> GetAllNeedResource()
        {
            for (var i = 0; i < needTypeList.Count; i++)
            {
                var resourceType = needTypeList[i];
                for (var j = 0; j < needValueList[i]; j++)
                {
                    yield return resourceType;
                }
            }
        }
    }
}