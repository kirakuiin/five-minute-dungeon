using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using GameLib.Common.Extension;
using Unity.Netcode;

namespace Gameplay.Core.Interactive
{
    /// <summary>
    /// 手牌选择器。
    /// </summary>
    public class HandSelector : NetworkBehaviour
    {
        /// <summary>
        /// 通知选择手牌。
        /// </summary>
        public event Action<int> OnSelectHandCard;
        
        private readonly NetworkVariable<bool> _isSelect =
            new (false, writePerm: NetworkVariableWritePermission.Owner);

        private NetworkList<int> _cardList;

        private void Awake()
        {
            _cardList = new NetworkList<int>(writePerm: NetworkVariableWritePermission.Owner);
        }

        /// <summary>
        /// 选择手牌。
        /// </summary>
        /// <param name="cardList"></param>
        public void SelectHand(IEnumerable<Card> cardList)
        {
            _isSelect.Value = true;
            foreach (var cardEnum in cardList)
            {
                _cardList.Add((int)cardEnum);
            }
        }
        
        /// <summary>
        /// 异步通知开始进行选择。
        /// </summary>
        /// <returns></returns>
        public async Task<List<Card>> GetSelectHandCards(int num)
        {
            GetSelectHandCardsClientRpc(num);
            await TaskExtension.Wait(() => _isSelect.Value);
            
            var result = new List<Card>();
            foreach (var cardEnum in _cardList)
            {
                result.Add((Card)cardEnum);
            }

            return result;
        }

        [Rpc(SendTo.Owner)]
        private void GetSelectHandCardsClientRpc(int num)
        {
            _isSelect.Value = false;
            _cardList.Clear();
            OnSelectHandCard?.Invoke(num);
        }
    }
}