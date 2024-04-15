using UnityEngine;

namespace UI.Card
{
    /// <summary>
    /// 不可打出的卡牌。
    /// </summary>
    public class UnplayableCard : MonoBehaviour
    {
        /// <summary>
        /// 初始化。
        /// </summary>
        public void Init(Data.Card card)
        {
            GetComponent<CardRuntimeData>().Init(card);
            GetComponent<CardAppearanceSetter>().Init();
        }
    }
}