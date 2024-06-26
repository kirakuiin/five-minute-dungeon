﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Instruction
{
    /// <summary>
    /// 服务端操控玩家的行动。
    /// </summary>
    public interface IPlayerController
    {
        /// <summary>
        /// 客户端ID。
        /// </summary>
        public ulong ClientID { get; }

        /// <summary>
        /// 手牌数量。
        /// </summary>
        public IReadOnlyList<Card> HandCards { get; }

        /// <summary>
        /// 打出卡牌。
        /// </summary>
        /// <param name="card"></param>
        public void Play(Card card);

        /// <summary>
        /// 从抽牌堆抽牌。
        /// </summary>
        /// <param name="num"></param>
        public void Draw(int num);

        /// <summary>
        /// 将手牌丢弃到弃牌区
        /// </summary>
        /// <param name="cards"></param>
        public void Discard(IEnumerable<Card> cards);

        /// <summary>
        /// 从弃牌堆顶端抽牌。
        /// </summary>
        /// <param name="num"></param>
        public void DrawFromDiscard(int num);

        /// <summary>
        /// 将弃牌区牌放到抽牌堆顶部。
        /// </summary>
        /// <param name="num"></param>
        public void DiscardToDraw(int num);

        /// <summary>
        /// 丢弃包含指定类型资源的资源卡。
        /// </summary>
        /// <param name="type"></param>
        public void DiscardResource(Resource type);

        /// <summary>
        /// 将手牌给予一名玩家。
        /// </summary>
        /// <param name="targetPlayerID"></param>
        public void GiveHand(ulong targetPlayerID);

        /// <summary>
        /// 将一堆牌加入手牌。
        /// </summary>
        /// <param name="cardList"></param>
        public void AddHand(IEnumerable<Card> cardList);

        /// <summary>
        /// 讲一堆牌加入抽牌堆。
        /// </summary>
        /// <param name="cardList"></param>
        public void AddDraw(IEnumerable<Card> cardList);

        /// <summary>
        /// 清理抽牌堆。
        /// </summary>
        public void CleanDrawPile();

        /// <summary>
        /// 补满手牌。
        /// </summary>
        public void FillHands();
        
        /// <summary>
        /// 获得玩家交互处理器。
        /// </summary>
        /// <returns></returns>
        public IPlayerInteractive GetInteractiveHandler();
    }

    /// <summary>
    /// 实现玩家交互相关操作。
    /// </summary>
    public interface IPlayerInteractive
    {
        /// <summary>
        /// 通知玩家选择玩家。
        /// </summary>
        /// <param name="num">选择数量</param>
        /// <param name="canSelectSelf">是否可以选择自己</param>
        /// <returns></returns>
        public Task<List<ulong>> SelectPlayers(int num, bool canSelectSelf);
        
        /// <summary>
        /// 通知玩家选择手牌。
        /// </summary>
        /// <param name="num">选择数量</param>
        /// <returns></returns>
        public Task<CancelableList<Card>> SelectHandCards(int num);

        /// <summary>
        /// 通知玩家选择敌方单位。
        /// </summary>
        /// <returns></returns>
        public Task<Cancelable<ulong>> SelectEnemy(EnemyCardType type);

        /// <summary>
        /// 通知玩家选择一种类型的资源。
        /// </summary>
        /// <returns></returns>
        public Task<Resource> SelectResource();
    }

    [Serializable]
    public struct CancelableList<T> where T : unmanaged
    {
        public bool isCancel;
        public List<T> array;

        public static CancelableList<T> Create(IEnumerable<T> list)
        {
            return new CancelableList<T>()
            {
                array = new List<T>(list),
                isCancel = false,
            };
        }

        public static CancelableList<T> CreateCancel()
        {
            return new CancelableList<T>()
            {
                array = default,
                isCancel = true,
            };
        }
    }

    [Serializable]
    public struct Cancelable<T> where T : unmanaged
    {
        public bool isCancel;
        public T elem;
        
        public static Cancelable<T> Create(T elem)
        {
            return new Cancelable<T>()
            {
                elem = elem,
                isCancel = false,
            };
        }

        public static Cancelable<T> CreateCancel()
        {
            return new Cancelable<T>()
            {
                elem = default,
                isCancel = true,
            };
        }
    }
}