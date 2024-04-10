using System.Collections.Generic;
using Data;
using Data.Instruction;
using GameLib.Common.DataStructure;

namespace Gameplay.Core
{
    /// <summary>
    /// 关卡控制。
    /// </summary>
    public class LevelController : ILevelController
    {
        private readonly Counter<Resource> _resPool = new();

        private readonly Dictionary<ulong, IEnemyCard> _enemyInfos = new();
        
        private readonly DungeonEnemyProvider _enemyProvider;

        private ulong _currentEnemyId;

        public LevelController(int playerNum, Boss bossType)
        {
            _enemyProvider = new(playerNum, DataService.Instance.GetBossData(bossType));
            Setup();
        }

        private void Setup()
        {
            RevealNextLevel(1);
        }
        
        public void AddResource(Resource type, int num = 1)
        {
            _resPool[type] += num;
        }

        public void DestroyEnemyCard(ulong enemyID)
        {
            if (_enemyInfos.ContainsKey(enemyID))
            {
                _enemyInfos.Remove(enemyID);
            }
        }

        public void RevealNextLevel(int num)
        {
            for (var i = 0; i < num; ++i)
            {
                if (_enemyProvider.IsReachBoss()) break;
                _enemyInfos[_currentEnemyId] = _enemyProvider.GetNextEnemyCard();
                _currentEnemyId += 1;
            }
        }

        public void StopTime()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<ulong> GetEnemyIDs()
        {
            throw new System.NotImplementedException();
        }
    }
}