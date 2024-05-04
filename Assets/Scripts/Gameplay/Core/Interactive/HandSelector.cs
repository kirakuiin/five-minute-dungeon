using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Check;
using Data.Instruction;
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

        public event Action OnHandSelectDone;

        private bool _isSelect;

        private CancelableList<Card> _cardList;

        private bool _isCancel;

        /// <summary>
        /// 选择手牌。
        /// </summary>
        /// <param name="cardList"></param>
        public void SelectHand(IEnumerable<Card> cardList)
        {
            SelectHandServerRpc(cardList.ToArray());
            OnHandSelectDone?.Invoke();
        }

        [Rpc(SendTo.Server)]
        private void SelectHandServerRpc(Card[] cards)
        {
            _cardList = CancelableList<Card>.Create(cards);
            _isSelect = true;
        }

        /// <summary>
        /// 取消选择手牌。
        /// </summary>
        public void CancelSelectHand()
        {
            CancelSelectHandServerRpc();
            OnHandSelectDone?.Invoke();
        }

        [Rpc(SendTo.Server)]
        private void CancelSelectHandServerRpc()
        {
            _cardList = CancelableList<Card>.CreateCancel();
            _isSelect = true;
        }
        
        /// <summary>
        /// 异步通知开始进行选择。
        /// </summary>
        /// <returns></returns>
        public async Task<CancelableList<Card>> GetSelectHandCards(int num)
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