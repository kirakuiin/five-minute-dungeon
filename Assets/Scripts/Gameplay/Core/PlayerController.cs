using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Data.Instruction;
using GameLib.Common;
using Gameplay.Core.Interactive;
using Gameplay.Data;
using Unity.Netcode;
using UnityEngine;

namespace Gameplay.Core
{
    /// <summary>
    /// 玩家控制器，用来同步玩家手牌信息。
    /// </summary>
    [RequireComponent(typeof(PlayerInteractive))]
    public class PlayerController : NetworkBehaviour, IPlayerController, IPlayerRuntimeInfo
    {
        private readonly HandsData _hands = new();
        private readonly DrawsData _draws = new();
        private readonly DiscardsData _discards = new();

        private void OnTransformParentChanged()
        {
            LocalSyncManager.Instance.SyncDone(LocalSyncInitStage.InitPlayerController);
        }

        public ulong ClientID => OwnerClientId;

        public Class PlayerClass { private set; get; }

        public void Init(IEnumerable<Card> cards, Class type)
        {
            InitClientRpc(cards.ToArray(), type);
        }

        [ClientRpc]
        private void InitClientRpc(Card[] cards, Class type)
        {
            PlayerClass = type;
            _draws.AddCard(cards);
            LocalSyncManager.Instance.SyncDone(LocalSyncInitStage.InitPile);
        }

        public void Draw(int num)
        {
            DrawClientRpc(num);
        }

        [ClientRpc]
        private void DrawClientRpc(int num)
        {
            _hands.AddCard(_draws.RemoveCard(num));
            if (!LocalSyncManager.Instance.HasBeenSyncDone(LocalSyncInitStage.InitHand))
            {
                LocalSyncManager.Instance.SyncDone(LocalSyncInitStage.InitHand);
            }
        }

        public void Discard(IEnumerable<Card> cards)
        {
            DiscardClientRpc(cards.ToArray());
        }
        
        [ClientRpc]
        private void DiscardClientRpc(Card[] cards)
        {
            _hands.RemoveCard(cards);
        }

        public void DrawFromDiscard(int num)
        {
            DrawFromDiscardClientRpc(num);
        }
        
        [ClientRpc]
        private void DrawFromDiscardClientRpc(int num)
        {
            _hands.AddCard(_discards.RemoveCard(num));
        }

        public void DiscardToDraw(int num)
        {
            DiscardToDrawClientRpc(num);
        }
        
        [ClientRpc]
        private void DiscardToDrawClientRpc(int num)
        {
            _draws.AddCard(_discards.RemoveCard(num));
        }

        public void DiscardResource(Resource type)
        {
            DiscardResourceClientRpc(type);
        }
        
        [ClientRpc]
        private void DiscardResourceClientRpc(Resource type)
        {
            var discardList = from card in _hands
                where DataHelper.IsCardContainResource(card, type)
                select card;

            _hands.RemoveCard(discardList.ToList());
        }

        public void GiveHand(ulong targetPlayerID)
        {
            GiveHandClientRpc(targetPlayerID);
        }
        
        [ClientRpc]
        private void GiveHandClientRpc(ulong targetPlayerID)
        {
            Debug.LogWarning("尚未实现手牌给予逻辑");
        }

        public void AddHand(IEnumerable<Card> cardList)
        {
            AddHandClientRpc(cardList.ToArray());
        }
        
        [ClientRpc]
        private void AddHandClientRpc(Card[] cardList)
        {
            _hands.AddCard(cardList);
        }

        public IPlayerInteractive GetInteractiveHandler()
        {
            return GetComponent<IPlayerInteractive>();
        }

        public ICardCollectionsInfo GetHands()
        {
            return _hands;
        }

        public ICardCollectionsInfo GetDraws()
        {
            return _draws;
        }

        public ICardCollectionsInfo GetDiscards()
        {
            return _discards;
        }
    }
}