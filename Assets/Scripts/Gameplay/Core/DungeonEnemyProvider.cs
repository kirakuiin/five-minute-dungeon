using System.Collections.Generic;
using System.Linq;
using Data;
using GameLib.Common.Extension;
using Random = System.Random;

namespace Gameplay.Core
{
    /// <summary>
    /// 提供地牢敌人。
    /// </summary>
    public class DungeonEnemyProvider
    {
        private readonly int _playerNum;

        private readonly BossData _bossData;

        private int _curEnemyIndex;

        private readonly List<EnemyCard> _enemyDeck = new ();

        /// <summary>
        /// 当前进度(从1开始)。
        /// </summary>
        public int CurProgress => _curEnemyIndex;

        /// <summary>
        /// 总关卡数量。
        /// </summary>
        public int TotalLevelNum => _enemyDeck.Count;
        
        public DungeonEnemyProvider(int playerNum, BossData data)
        {
            _playerNum = playerNum;
            _bossData = data;
            Setup();
        }

        private void Setup()
        {
            BuildDeck();
        }

        private void BuildDeck()
        {
            _curEnemyIndex = 0;
            _enemyDeck.Clear();
            AddDoorCard();
            AddChallengeCard();
            TotalShuffle();
            _enemyDeck.Add(new EnemyCard() {type = EnemyCardType.Boss, value = (int)_bossData.boss});
        }

        private void AddDoorCard()
        {
            var random = new Random();
            var doorCardList = (from data in DataService.Instance.GetAllDoorData()
                select new EnemyCard() { type = data.enemyCardType, value = (int)data.card }).ToList();
            _enemyDeck.AddRange(random.Sample(doorCardList, _bossData.doorCardNum));
            
        }

        private void AddChallengeCard()
        {
            var random = new Random();
            var challengeCardList = (from data in DataService.Instance.GetAllChallengeCard()
                select new EnemyCard() { type = data.enemyCardType, value = (int)data.card }).ToList();
            var k = _playerNum * 2 + _bossData.challengeNum;
            _enemyDeck.AddRange(random.Sample(challengeCardList, k));
        }

        private void TotalShuffle()
        {
            var random = new Random();
            random.Shuffle(_enemyDeck);
        }

        /// <summary>
        /// 是否到达关底
        /// </summary>
        /// <returns></returns>
        public bool IsReachBoss()
        {
            return _curEnemyIndex == _enemyDeck.Count-1;
        }
        
        /// <summary>
        /// 获得下一个需要挑战的敌人。
        /// </summary>
        /// <returns></returns>
        public EnemyCard GetNextEnemyCard()
        {
            var result = _enemyDeck[_curEnemyIndex];
            _curEnemyIndex += 1;
            return result;
        }

        /// <summary>
        /// 新增敌人
        /// </summary>
        public void AddEnemy(EnemyCard card)
        {
            _enemyDeck.Add(card);
        }

        /// <summary>
        /// 清理当前敌人以外的全部敌人。
        /// </summary>
        public void ClearAllEnemyExceptCurrent()
        {
            _enemyDeck.RemoveRange(_curEnemyIndex, TotalLevelNum-_curEnemyIndex);
        }
    }
}