using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using GameLib.Common.Extension;
using Unity.Netcode;

namespace Gameplay.Core.Interactive
{
    /// <summary>
    /// 玩家选择器。
    /// </summary>
    public class PlayerSelector : Selector
    {
        /// <summary>
        /// 通知选择玩家对象。
        /// </summary>
        public event Action<int, bool> OnSelectPlayer;
        
        private readonly NetworkVariable<bool> _isSelect =
            new NetworkVariable<bool>(false, writePerm: NetworkVariableWritePermission.Owner);

        private NetworkList<ulong> _playerList;

        private void Awake()
        {
            _playerList = new NetworkList<ulong>(writePerm: NetworkVariableWritePermission.Owner);
        }

        /// <summary>
        /// 选择玩家。
        /// </summary>
        /// <param name="playerList"></param>
        public void SelectPlayer(IEnumerable<ulong> playerList)
        {
            _isSelect.Value = true;
            foreach (var id in playerList)
            {
                _playerList.Add(id);
            }
        }
        
        /// <summary>
        /// 异步通知开始进行选择。
        /// </summary>
        /// <returns></returns>
        public async Task<List<ulong>> GetSelectPlayerList(int num, bool canSelectSelf)
        {
            GetSelectPlayerListClientRpc(num, canSelectSelf, GetClientParam());
            await TaskExtension.Wait(() => _isSelect.Value);
            
            var result = new List<ulong>();
            foreach (var id in _playerList)
            {
                result.Add(id);
            }

            return result;
        }

        [ClientRpc]
        private void GetSelectPlayerListClientRpc(int num, bool canSelectSelf, ClientRpcParams rpcParams=default)
        {
            _isSelect.Value = false;
            _playerList.Clear();
            OnSelectPlayer?.Invoke(num, canSelectSelf);
        }
    }
}