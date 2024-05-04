using UnityEngine;

namespace Data
{

    /// <summary>
    /// 牌组内容修正。
    /// </summary>
    /// <remarks>根据玩家的数量对牌组进行一定调整。</remarks>
    [CreateAssetMenu(fileName = "DeckModifier", menuName = "数据/牌组/修改器", order = 1)]
    public class DeckModifier : DictionaryScriptObj<Card, int>
    {
        public int playerNum;
    }
}