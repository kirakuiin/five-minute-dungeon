using GameLib.Common;
using GameLib.UI.SectorLayout;
using Gameplay.Core;
using UI.Card;
using UnityEngine;

namespace UI.Gameplay
{
    /// <summary>
    /// 手牌UI控制。
    /// </summary>
    public class HandZoneUIController : MonoBehaviour
    {
        [SerializeField] private SectorLayout layout;

        [SerializeField] private GameObject cardPrefab;

        private IPlayerRuntimeInfo _info;

        public void Init(IPlayerRuntimeInfo info)
        {
            _info = info;
            InitUI();
        }

        private void InitUI()
        {
            foreach (var card in _info.GetHands())
            {
                layout.Add(CreateCardObj(card));
            }
        }
        
        private GameObject CreateCardObj(Data.Card card)
        {
            var cardObj = GameObjectPool.Instance.Get(cardPrefab);
            var setter = cardObj.GetComponent<PlayableCard>();
            setter.Init(card, this);
            return cardObj;
        }

        /// <summary>
        /// 移除卡牌。
        /// </summary>
        /// <param name="cardObj"></param>
        public void RemoveCard(GameObject cardObj)
        {
            layout.Remove(cardObj);
            cardObj.transform.SetParent(GameObjectPool.Instance.transform);
            GameObjectPool.Instance.Return(cardObj, cardPrefab); 
        }
    }
}