using System;
using System.Threading.Tasks;
using Common;
using GameLib.Common.Extension;
using Unity.Netcode;

namespace Gameplay.Core.Interactive
{
    /// <summary>
    /// 敌人选择器。
    /// </summary>
    public class EnemySelector : Selector
    {
        /// <summary>
        /// 通知选择敌对单位。
        /// </summary>
        public event Action OnSelectEnemy;

        private readonly NetworkVariable<ulong> _enemyID =
            new NetworkVariable<ulong>(writePerm: NetworkVariableWritePermission.Owner);

        private readonly NetworkVariable<bool> _isSelect =
            new NetworkVariable<bool>(false, writePerm: NetworkVariableWritePermission.Owner);

        /// <summary>
        /// 选择敌方单位。
        /// </summary>
        /// <param name="enemyID"></param>
        public void SelectEnemy(ulong enemyID)
        {
            _isSelect.Value = true;
            _enemyID.Value = enemyID;
        }
        
        /// <summary>
        /// 异步通知开始进行选择。
        /// </summary>
        /// <returns></returns>
        public async Task<ulong> GetSelectEnemyID()
        {
            GetSelectEnemyIDClientRpc(GetClientParam());
            await TaskExtension.Wait(() => _isSelect.Value);
            return _enemyID.Value;
        }

        [ClientRpc]
        private void GetSelectEnemyIDClientRpc(ClientRpcParams param=default)
        {
            _isSelect.Value = false;
            OnSelectEnemy?.Invoke();
        }
    }
}