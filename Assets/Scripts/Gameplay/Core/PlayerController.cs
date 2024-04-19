using System.Collections.Generic;
using System.Linq;
using Data;
using Data.Check;
using Data.Instruction;
using GameLib.Common;
using Gameplay.Core.Interactive;
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

        [Rpc(SendTo.ClientsAndHost)]
        private void InitClientRpc(Card[] cards, Class type)
        {
            PlayerClass = type;
            _draws.AddCard(cards);
            LocalSyncManager.Instance.SyncDone(LocalSyncInitStage.InitPile);
        }

        public void Play(Card card)
        {
            PlayClientRpc(card);
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void PlayClientRpc(Card card)
        {
            _hands.RemoveCard(new [] {card});
        }

        public void Draw(int num)
        {
            DrawClientRpc(num);
        }

        [Rpc(SendTo.ClientsAndHost)]
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
        
        [Rpc(SendTo.ClientsAndHost)]
        private void DiscardClientRpc(Card[] cards)
        {
            _hands.RemoveCard(cards);
            _discards.AddCard(cards);
        }

        public void DrawFromDiscard(int num)
        {
            DrawFromDiscardClientRpc(num);
        }
        
        [Rpc(SendTo.ClientsAndHost)]
        private void DrawFromDiscardClientRpc(int num)
        {
            _hands.AddCard(_discards.RemoveCard(num));
        }

        public void DiscardToDraw(int num)
        {
            DiscardToDrawClientRpc(num);
        }
        
        [Rpc(SendTo.ClientsAndHost)]
        private void DiscardToDrawClientRpc(int num)
        {
            _draws.AddCard(_discards.RemoveCard(num));
        }

        public void DiscardResource(Resource type)
        {
            DiscardResourceClientRpc(type);
        }
        
        [Rpc(SendTo.ClientsAndHost)]
        private void DiscardResourceClientRpc(Resource type)
        {
            var discardList = (from card in _hands
                where DataHelper.IsCardContainResource(card, type)
                select card).ToList();

            _hands.RemoveCard(discardList);
            _discards.AddCard(discardList);
        }

        public void GiveHand(ulong targetPlayerID)
        {
            GiveHandClientRpc(targetPlayerID);
        }
        
        [Rpc(SendTo.ClientsAndHost)]
        private void GiveHandClientRpc(ulong targetPlayerID)
        {
            Debug.LogWarning("尚未实现手牌给予逻辑");
        }

        public void AddHand(IEnumerable<Card> cardList)
        {
            AddHandClientRpc(cardList.ToArray());
        }
        
        [Rpc(SendTo.ClientsAndHost)]
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