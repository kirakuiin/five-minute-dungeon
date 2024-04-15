using UI.Gameplay;
using UnityEngine;

namespace UI.Card
{
    /// <summary>
    /// 代表可以打出的手牌。
    /// </summary>
    public class PlayableCard : MonoBehaviour
    {
        private HandZoneUIController _handZone;
        
        /// <summary>
        /// 初始化。
        /// </summary>
        public void Init(Data.Card card, HandZoneUIController zone)
        {
            _handZone = zone;
            GetComponent<CardRuntimeData>().Init(card);
            GetComponent<CardAppearanceSetter>().Init();
        }
        
        /// <summary>
        /// 打出卡牌。
        /// </summary>
        public void PlayCard()
        {
        }

        /// <summary>
        /// 丢弃卡牌。
        /// </summary>
        public void DiscardCard()
        {
            RemoveCard();
            // TODO(nico): 通过一个专门的对象实现同服务器的通信，比如弃牌，打牌，放技能等等。
        }
        
        private void RemoveCard()
        {
            _handZone.RemoveCard(gameObject);
        }

    }
}