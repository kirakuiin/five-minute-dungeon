﻿using System;
using System.Threading.Tasks;
using Data;
using Data.Check;
using Data.Instruction;
using GameLib.Common.Extension;
using Unity.Netcode;

namespace Gameplay.Core.Interactive
{
    /// <summary>
    /// 敌人选择器。
    /// </summary>
    public class EnemySelector : NetworkBehaviour, IEnemySelector
    {
        /// <summary>
        /// 通知选择敌对单位。
        /// </summary>
        public event Action<EnemyCardType> OnEnemySelecting;

        public event Action OnSelectDone;

        private Cancelable<ulong> _enemyID;

        private bool _isSelect;

        /// <summary>
        /// 选择敌方单位。
        /// </summary>
        /// <param name="enemyID"></param>
        public void SelectEnemy(ulong enemyID)
        {
            SelectEnemyServerRpc(enemyID);
            OnSelectDone?.Invoke();
        }

        [Rpc(SendTo.Server)]
        private void SelectEnemyServerRpc(ulong enemyID)
        {
            _enemyID = Cancelable<ulong>.Create(enemyID);
            _isSelect = true;
        }
        
        /// <summary>
        /// 取消选择。
        /// </summary>
        public void CancelSelectEnemy()
        {
            CancelSelectEnemyRpc();
            OnSelectDone?.Invoke();
        }

        [Rpc(SendTo.Server)]
        private void CancelSelectEnemyRpc()
        {
            _enemyID = Cancelable<ulong>.CreateCancel();
            _isSelect = true;
        }
        
        /// <summary>
        /// 异步通知开始进行选择。
        /// </summary>
        /// <returns></returns>
        public async Task<Cancelable<ulong>> GetSelectEnemyID(EnemyCardType type)
        {
            _isSelect = false;
            GetSelectEnemyIDClientRpc(type);
            await TaskExtension.Wait(() => _isSelect);
            return _enemyID;
        }

        [Rpc(SendTo.Owner)]
        private void GetSelectEnemyIDClientRpc(EnemyCardType type)
        {
            OnEnemySelecting?.Invoke(type);
        }

        public override void OnNetworkDespawn()
        {
            _enemyID = Cancelable<ulong>.CreateCancel();
            _isSelect = true;
        }
    }
}