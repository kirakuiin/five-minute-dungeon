using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Check;
using GameLib.Common.Extension;
using Unity.Netcode;

namespace Gameplay.Core.Interactive
{
    /// <summary>
    /// 手牌选择器。
    /// </summary>
    public class HandSelector : NetworkBehaviour, IHandSelector
    {
        /// <summary>
        /// 通知选择手牌。
        /// </summary>
        public event Action<int> OnHandSelecting;

        private bool _isSelect;

        private List<Card> _cardList;

        /// <summary>
        /// 选择手牌。
        /// </summary>
        /// <param name="cardList"></param>
        public void SelectHand(IEnumerable<Card> cardList)
        {
            SelectHandServerRpc(cardList.ToArray());
        }

        [Rpc(SendTo.Server)]
        private void SelectHandServerRpc(Card[] cards)
        {
            _cardList = new(cards);
            _isSelect = true;
        }
        
        /// <summary>
        /// 异步通知开始进行选择。
        /// </summary>
        /// <returns></returns>
        public async Task<List<Card>> GetSelectHandCards(int num)
        {
            _isSelect = false;
            GetSelectHandCardsClientRpc(num);
            await TaskExtension.Wait(() => _isSelect);
            return _cardList;
        }

        [Rpc(SendTo.Owner)]
        private void GetSelectHandCardsClientRpc(int num)
        {
            OnHandSelecting?.Invoke(num);
        }
    }
}