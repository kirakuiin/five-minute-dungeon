using System;
using GameLib.Common;
using UnityEngine;

namespace UI.Card
{
    /// <summary>
    /// 卡牌丢弃效果。
    /// </summary>
    public class CardDiscardEffect : MonoBehaviour
    {
        [SerializeField] private GameObject cardPrefab;

        [SerializeField] private Transform destination;

        [SerializeField] private Transform parent;

        /// <summary>
        /// 丢弃卡牌。
        /// </summary>
        public void Discard(GameObject cardObj)
        {
            var obj = GameObjectPool.Instance.Get(cardPrefab);
            obj.transform.position = cardObj.transform.position;
            obj.transform.SetParent(parent);
            obj.GetComponent<DiscardCardObj>().Init(cardPrefab, cardObj.GetComponent<ICardData>().Card);
            obj.GetComponent<DiscardCardObj>().MoveToTarget(destination);
        }
    }
}