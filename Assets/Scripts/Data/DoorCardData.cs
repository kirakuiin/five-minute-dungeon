using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "DoorCardData", menuName = "数据/卡牌数据/门卡", order = 1)]
    public class DoorCardData: DictionaryScriptObj<Resource, int>
    {
        [Tooltip("卡牌枚举")]
        public Door card;
        
        [Tooltip("卡牌类型")]
        public EnemyCardType enemyCardType;
    }
}