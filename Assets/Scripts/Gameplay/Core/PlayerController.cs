using System.Collections.Generic;
using System.Linq;
using Data;
using Data.Check;
using Data.Instruction;
using GameLib.Common;
using GameLib.Network.NGO;
using Gameplay.Core.Interactive;
using Gameplay.Data;
using Unity.Mathematics;
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

        public int PlayCardNum { private set; get; } = 0;

        public Class PlayerClass { private set; get; } = Class.Wizard;

        public string PlayerName { private set; get; } = "";

        public bool IsHaveCards => _hands.Count + _draws.Count > 0;

        protected override void OnSynchronize<T>(ref BufferSerializer<T> serializer)
        {
            if (serializer.IsWriter)
            {
                var writer = serializer.GetFastBufferWriter();
                writer.WriteValueSafe(_hands);
                writer.WriteValueSafe(_draws);
                writer.WriteValueSafe(_discards);
                writer.WriteValueSafe(PlayerClass);
                writer.WriteValueSafe(PlayerName);
                writer.WriteValueSafe(PlayCardNum);
            }
            else
            {
                var reader = serializer.GetFastBufferReader();
                reader.ReadValueSafe(out HandsData hands);
                _hands.AddCard(hands);
                reader.ReadValueSafe(out HandsData draws);
                _draws.AddCard(draws);
                reader.ReadValueSafe(out HandsData discards);
                _discards.AddCard(discards);
                reader.ReadValueSafe(out Class playerClass);
                reader.ReadValueSafe(out string playerName);
                reader.ReadValueSafe(out int playedNum);
                PlayerClass = playerClass;
                PlayerName = playerName;
                PlayCardNum = playedNum;
            }
        }

        private void OnTransformParentChanged()
        {
            LocalSyncManager.Instance.SyncDone(LocalSyncInitStage.InitPlayerController);
        }

        public ulong ClientID => OwnerClientId;

        public IReadOnlyList<Card> HandCards => _hands.ToList();
        
        /// <summary>
        /// 设置手牌和弃牌区。
        /// </summary>
        /// <param name="hands"></param>
        /// <param name="discards"></param>
        public void SetHandAndDiscard(IEnumerable<Card> hands, IEnumerable<Card> discards)
        {
            var discardList = discards.ToList();
            SetFirstHand(hands.Concat(discardList));
            Discard(discardList);
        }
        
        /// <summary>
        /// 设置初始手牌。
        /// </summary>
        public void SetFirstHand(IEnumerable<Card> cards)
        {
            SetFirstHandRpc(cards.ToArray());
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void SetFirstHandRpc(Card[] cards)
        {
            _hands.AddCard(cards.ToList());
            LocalSyncManager.Instance.SyncDone(LocalSyncInitStage.InitPile);
        }
        
        public void Init(IEnumerable<Card> cards, Class type, string playerName)
        {
            AddDraw(cards);
            InitClientRpc(type, playerName);
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void InitClientRpc(Class type, string playerName)
        {
            PlayerClass = type;
            PlayerName = playerName;
        }

        public void Play(Card card)
        {
            PlayClientRpc(card);
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void PlayClientRpc(Card card)
        {
            PlayCardNum += 1;
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
            var targetPlayer = GamePlayContext.Instance.GetPlayerController(targetPlayerID);
            targetPlayer.AddHand(HandCards);
            RemoveHandClientRpc();
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void RemoveHandClientRpc()
        {
            _hands.RemoveCard(_hands.Count);
        }

        public void AddHand(IEnumerable<Card> cardList)
        {
            AddHandClientRpc(cardList.ToArray());
        }

        public void AddDraw(IEnumerable<Card> cardList)
        {
            AddDrawClientRpc(cardList.ToArray());
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void AddDrawClientRpc(Card[] cardList)
        {
            _draws.AddCard(cardList);
        }

        public void CleanDrawPile()
        {
            CleanDrawPileClientRpc();
        }

        public void FillHands()
        {
            FillHandsRpc();
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void FillHandsRpc()
        {
            var diff = GamePlayContext.Instance.InitHandNum - _hands.Count;
            diff = math.min(diff, _draws.Count);
            if (diff > 0)
            {
                _hands.AddCard(_draws.RemoveCard(diff));
            }
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void CleanDrawPileClientRpc()
        {
            _draws.RemoveCard(_draws.Count);
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

        public IRuntimeInteractive GetRuntimeInteractive()
        {
            return GetComponent<IRuntimeInteractive>();
        }

        public override void OnNetworkDespawn()
        {
            if (!IsServer) return;
            var manager = SessionManager<PlayerSessionData>.Instance;
            var data = manager.GetPlayerData(OwnerClientId);
            if (data.HasValue)
            {
                var newData = data.Value;
                newData.DrawData = new List<Card>(_draws);
                newData.DiscardData = new List<Card>(_discards);
                newData.HandData = new List<Card>(_hands);
                manager.UpdatePlayerData(newData.ClientID, newData);
            }
        }
    }
}