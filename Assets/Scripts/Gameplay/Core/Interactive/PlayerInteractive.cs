using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Data.Instruction;
using Unity.Netcode;
using UnityEngine;

namespace Gameplay.Core.Interactive
{
    /// <summary>
    /// 玩家交互控制器。
    /// </summary>
    [RequireComponent(typeof(PlayerController))]
    public class PlayerInteractive : MonoBehaviour, IPlayerInteractive
    {
        [SerializeField] private EnemySelector enemySelector;
        [SerializeField] private ResourceSelector resourceSelector;
        [SerializeField] private PlayerSelector playerSelector;
        [SerializeField] private HandSelector handSelector;
        
        public async Task<List<ulong>> SelectPlayers(int num, bool canSelectSelf)
        {
            return await playerSelector.GetSelectPlayerList(num, canSelectSelf);
        }

        public async Task<List<Card>> SelectHandCards(int num)
        {
            return await handSelector.GetSelectHandCards(num);
        }
        

        public async Task<ulong> SelectEnemy()
        {
            return await enemySelector.GetSelectEnemyID();
        }
        
        public async Task<Resource> SelectResource()
        {
            return await resourceSelector.GetSelectRes();
        }
    }
}