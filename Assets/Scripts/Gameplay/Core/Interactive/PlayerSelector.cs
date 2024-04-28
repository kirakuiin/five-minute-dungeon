using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Check;
using GameLib.Common.Extension;
using Unity.Netcode;

namespace Gameplay.Core.Interactive
{
    /// <summary>
    /// 玩家选择器。
    /// </summary>
    public class PlayerSelector : NetworkBehaviour, IPlayerSelector
    {
        /// <summary>
        /// 通知选择玩家对象。
        /// </summary>
        public event Action<int, bool> OnPlayerSelecting;

        public event Action OnSelectDone;

        private bool _isSelect;

        private List<ulong> _playerList;

        /// <summary>
        /// 选择玩家。
        /// </summary>
        /// <param name="playerList"></param>
        public void SelectPlayer(IEnumerable<ulong> playerList)
        {
            SelectPlayerServerRpc(playerList.ToArray());
            OnSelectDone?.Invoke();
        }

        [Rpc(SendTo.Server)]
        private void SelectPlayerServerRpc(ulong[] playerList)
        {
            _playerList = new(playerList);
            _isSelect = true;
        }
        
        /// <summary>
        /// 异步通知开始进行选择。
        /// </summary>
        /// <returns></returns>
        public async Task<List<ulong>> GetSelectPlayerList(int num, bool canSelectSelf)
        {
            _isSelect = false;
            GetSelectPlayerListClientRpc(num, canSelectSelf);
            await TaskExtension.Wait(() => _isSelect);
            
            return _playerList;
        }

        [Rpc(SendTo.Owner)]
        private void GetSelectPlayerListClientRpc(int num, bool canSelectSelf)
        {
            OnPlayerSelecting?.Invoke(num, canSelectSelf);
        }
    }
}