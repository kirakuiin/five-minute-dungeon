using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Data.Check;
using Data.Instruction;
using UnityEngine;

namespace Gameplay.Core.Interactive
{
    /// <summary>
    /// 玩家交互控制器。
    /// </summary>
    [RequireComponent(typeof(PlayerController))]
    public class PlayerInteractive : MonoBehaviour, IPlayerInteractive, IRuntimeInteractive
    {
        [SerializeField] private EnemySelector enemySelector;
        [SerializeField] private ResourceSelector resourceSelector;
        [SerializeField] private PlayerSelector playerSelector;
        [SerializeField] private HandSelector handSelector;
        
        public async Task<List<ulong>> SelectPlayers(int num, bool canSelectSelf)
        {
            return await playerSelector.GetSelectPlayerList(num, canSelectSelf);
        }

        public async Task<CancelableList<Card>> SelectHandCards(int num)
        {
            return await handSelector.GetSelectHandCards(num);
        }
        

        public async Task<Cancelable<ulong>> SelectEnemy(EnemyCardType type)
        {
            return await enemySelector.GetSelectEnemyID(type);
        }
        
        public async Task<Resource> SelectResource()
        {
            return await resourceSelector.GetSelectRes();
        }

        public IEnemySelector GetEnemySelector()
        {
            return enemySelector;
        }

        public IResourceSelector GetResourceSelector()
        {
            return resourceSelector;
        }

        public IPlayerSelector GetPlayerSelector()
        {
            return playerSelector;
        }

        public IHandSelector GetHandSelector()
        {
            return handSelector;
        }
    }
}