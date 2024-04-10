using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "DoorCardData", menuName = "数据/卡牌数据/门卡", order = 1)]
    public class DoorCardData: DictionaryScriptObj<Resource, int>, IEnemyCard
    {
        [Tooltip("卡牌枚举")]
        public Door card;
        
        [Tooltip("卡牌类型")]
        public EnemyCardType enemyCardType;

        public EnemyCardType Type => enemyCardType;

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