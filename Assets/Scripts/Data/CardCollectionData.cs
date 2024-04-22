using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace Data
{
    /// <summary>
    /// 卡牌变化类型。
    /// </summary>
    public enum CardChangeType
    {
        AddCard,
        RemoveCard,
    }

    /// <summary>
    /// 卡牌变化事件。
    /// </summary>
    public struct CardChangeEvent
    {
        public CardChangeType type;
        public List<Card> cardList;

        /// <summary>
        /// 创建一个增加手牌事件。
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static CardChangeEvent CreateAddEvent(IEnumerable<Card> cards)
        {
            return new CardChangeEvent()
            {
                type = CardChangeType.AddCard,
                cardList = new List<Card>(cards),
            };
        }
        
        /// <summary>
        /// 创建一个移除手牌事件。
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static CardChangeEvent CreateRemoveEvent(IEnumerable<Card> cards)
        {
            return new CardChangeEvent()
            {
                type = CardChangeType.RemoveCard,
                cardList = new List<Card>(cards),
            };
        }
    }
    
    public interface ICardCollections : INetworkSerializable, IEnumerable<Card>
    {
        /// <summary>
        /// 牌的数量。
        /// </summary>
        public int Count { get; }
        
        /// <summary>
        /// 添加卡牌。
        /// </summary>
        /// <param name="cards"></param>
        public void AddCard(IEnumerable<Card> cards);

        /// <summary>
        /// 移除指定卡牌。
        /// </summary>
        /// <param name="cards"></param>
        public void RemoveCard(IEnumerable<Card> cards);

        /// <summary>
        /// 移除指定数量的卡牌。
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public IEnumerable<Card> RemoveCard(int num);

        void INetworkSerializable.NetworkSerialize<T>(BufferSerializer<T> serializer)
        {
            if (serializer.IsReader)
            {
                var reader = serializer.GetFastBufferReader();
                reader.ReadValueSafe(out int count);

                var cards = new List<Card>();
                for (int i = 0; i < count; i++)
                {
                    reader.ReadValueSafe(out Card card);
                    cards.Add(card);
                }
                
                RemoveCard(Count);
                AddCard(cards);
            }

            if (serializer.IsWriter)
            {
                var writer = serializer.GetFastBufferWriter();
                writer.WriteValueSafe(Count);
                foreach (var card in this)
                {
                    writer.WriteValueSafe(card);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public interface ICardCollectionsInfo : IEnumerable<Card>
    {
        /// <summary>
        /// 牌的数量。
        /// </summary>
        public int Count { get; }

        /// <summary>
        /// 当牌组内容发生变化时触发。
        /// </summary>
        public event Action<CardChangeEvent> OnCardChanged;
    }

    /// <summary>
    /// 手牌区。
    /// </summary>
    public class HandsData : ICardCollections, ICardCollectionsInfo
    {
        private readonly List<Card> _hands = new();

        public int Count => _hands.Count;
        
        public event Action<CardChangeEvent> OnCardChanged;

        public void AddCard(IEnumerable<Card> cards)
        {
            var list = cards.ToList();
            foreach (var card in list)
            {
                _hands.Add(card);
            }
            if (list.Count > 0)
            {
                OnCardChanged?.Invoke(CardChangeEvent.CreateAddEvent(list));
            }
        }

        public void RemoveCard(IEnumerable<Card> cards)
        {
            var list = cards.ToList();
            foreach (var card in list)
            {
                if (!_hands.Remove(card))
                {
                    Debug.LogWarning($"移除不存在的卡牌{card}");
                }
            }

            if (list.Count > 0)
            {
                OnCardChanged?.Invoke(CardChangeEvent.CreateRemoveEvent(list));
            }
        }

        public IEnumerable<Card> RemoveCard(int num)
        {
            var realRemoveCount = Math.Min(num, _hands.Count);
            var result = _hands.GetRange(0, realRemoveCount);
            _hands.RemoveRange(0, realRemoveCount);
            if (result.Count > 0)
            {
                OnCardChanged?.Invoke(CardChangeEvent.CreateRemoveEvent(result));
            }
            return result;
        }

        public IEnumerator<Card> GetEnumerator()
        {
            return _hands.GetEnumerator();
        }
    }

    /// <summary>
    /// 抽牌区。
    /// </summary>
    public class DrawsData : ICardCollections, ICardCollectionsInfo
    {
        private readonly Stack<Card> _pile = new();

        public int Count => _pile.Count;
        
        public event Action<CardChangeEvent> OnCardChanged;

        public void AddCard(IEnumerable<Card> cards)
        {
            var list = cards.ToList();
            foreach (var card in list)
            {
                _pile.Push(card);
            }

            if (list.Count > 0)
            {
                OnCardChanged?.Invoke(CardChangeEvent.CreateAddEvent(list));
            }
        }

        public void RemoveCard(IEnumerable<Card> cards)
        {
            Debug.LogError("不支持的操作。");
        }

        public IEnumerable<Card> RemoveCard(int num)
        {
            var result = (from _ in Enumerable.Range(0, Math.Min(_pile.Count, num))
                select _pile.Pop()).ToList();
            if (result.Count > 0)
            {
                OnCardChanged?.Invoke(CardChangeEvent.CreateRemoveEvent(result));
            }
            return result;
        }

        public IEnumerator<Card> GetEnumerator()
        {
            return _pile.Reverse().GetEnumerator();
        }
    }

    /// <summary>
    /// 弃牌区。
    /// </summary>
    public class DiscardsData : DrawsData
    {
    }

    /// <summary>
    /// 放逐区。
    /// </summary>
    public class ExileData : HandsData
    {
    }
}