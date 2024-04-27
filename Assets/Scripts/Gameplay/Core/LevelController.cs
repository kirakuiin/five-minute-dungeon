﻿using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Data.Check;
using Data.Instruction;
using GameLib.Common.DataStructure;
using Unity.Netcode;

namespace Gameplay.Core
{
    /// <summary>
    /// 关卡控制。
    /// </summary>
    public class LevelController : NetworkBehaviour, ILevelController, ILevelRuntimeInfo
    {
        private readonly Dictionary<ulong, EnemyCard> _enemyInfos = new();
        private readonly Counter<Resource> _resPool = new();
        private readonly NetworkVariable<int> _curProgress = new();
        private readonly NetworkVariable<int> _totalLevelNum = new();
        
        // server only
        private DungeonEnemyProvider _enemyProvider;
        private ulong _currentEnemyId;

        public event Action<EnemyChangeEvent> OnEnemyAdded;
        public event Action<EnemyChangeEvent> OnEnemyDestroyed;
        public event Action<Resource, int> OnResourceAdded;

        public int CurProgress => _curProgress.Value;

        public int TotalLevelNum => _totalLevelNum.Value;
        
        public IEnumerable<Resource> GetCurNeedResources()
        {
            var tempList = new List<Resource>();
            foreach (var enemyCard in _enemyInfos.Values)
            {
                var result = GetAllNeedResourceByCard(enemyCard);
                tempList.AddRange(result.Elements());
            }

            return new Counter<Resource>(tempList).Elements();
        }

        private Counter<Resource> GetAllNeedResourceByCard(EnemyCard card)
        {
            EnemyScriptObj data = DataService.Instance.GetEnemyCardData(card);
            
            var result = new Counter<Resource>();
            foreach (var key in data.needTypeList)
            {
                result[key] += data.Get(key);
            }

            return result;
        }

        public IEnumerable<Resource> GetAlreadyPlayedResources()
        {
            return _resPool.Elements();
        }

        public IReadOnlyDictionary<ulong, EnemyCard> GetAllEnemiesInfo()
        {
            return _enemyInfos;
        }

        /// <summary>
        /// 初始化。
        /// </summary>
        /// <param name="playerNum"></param>
        /// <param name="bossType"></param>
        public void Setup(int playerNum, Boss bossType)
        {
            _enemyProvider = new DungeonEnemyProvider(playerNum, DataService.Instance.GetBossData(bossType));
            _totalLevelNum.Value = _enemyProvider.TotalLevelNum;
        }
        
        public void AddResource(Resource type, int num = 1)
        {
            AddResourceClientRpc(type, num);
            ProcessLevelResource();
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void AddResourceClientRpc(Resource type, int num)
        {
            _resPool[type] += num;
            OnResourceAdded?.Invoke(type, num);
        }

        private void ProcessLevelResource()
        {
            if (!IsPlayedResourceGeThanNeeded()) return;
            foreach (var enemyID in _enemyInfos.Keys.ToList())
            {
                DestroyEnemyCard(enemyID);
            }
        }

        private bool IsPlayedResourceGeThanNeeded()
        {   
            if ((this as ILevelRuntimeInfo).IsContainEvent()) return false;
            var curRes = new Counter<Resource>(GetAlreadyPlayedResources());
            curRes.Subtract(new Counter<Resource>(GetCurNeedResources()));
            var negativeSum = (from val in curRes.Values where val < 0 select val).Sum();
            return curRes[Resource.Wild] >= -negativeSum;
        }

        public void DestroyEnemyCard(ulong enemyID)
        {
            if (!_enemyInfos.ContainsKey(enemyID)) return;
            DestroyEnemyClientRpc(enemyID);
            var noEnemy = GetAllEnemiesInfo().Count == 0;
            if (noEnemy)
            {
                ClearResourcePoolClientRpc();
            }
            else
            {
                ProcessLevelResource();
            }
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void ClearResourcePoolClientRpc()
        {
            _resPool.Clear();
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void DestroyEnemyClientRpc(ulong enemyID)
        {
            if (!_enemyInfos.ContainsKey(enemyID)) return;
            var enemyCard = _enemyInfos[enemyID];
            _enemyInfos.Remove(enemyID);
            OnEnemyDestroyed?.Invoke(new EnemyChangeEvent() {enemyCard = enemyCard, enemyID = enemyID});
        }

        public bool IsReachBoss() => _enemyProvider.IsReachBoss();
        
        public bool IsComplete() => CurProgress >= TotalLevelNum;

        public void RevealNextLevel(int num)
        {
            for (var i = 0; i < num; ++i)
            {
                if (_enemyProvider.IsReachBoss()) break;
                RevealOne();
            }
        }

        private void RevealOne()
        {
            var enemy = _enemyProvider.GetNextEnemyCard();
            _curProgress.Value = _enemyProvider.CurProgress;
            AddOneEnemyClientRpc(_currentEnemyId,
                new EnemyCardWrapper{ value = enemy });
            _currentEnemyId += 1;
        }

        public void RevealBoss()
        {
            if (IsReachBoss())
            {
                RevealOne();
            }
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void AddOneEnemyClientRpc(ulong enemyID, EnemyCardWrapper wrapper)
        {
            _enemyInfos[enemyID] = wrapper.value;
            OnEnemyAdded?.Invoke(new EnemyChangeEvent() {enemyID = enemyID, enemyCard = wrapper.value});
        }
        
        // GM使用

        public void AddEnemy(EnemyCard card)
        {
            _enemyProvider.AddEnemy(card);
            _totalLevelNum.Value = _enemyProvider.TotalLevelNum;
        }

        public void ClearAllEnemy()
        {
            _enemyProvider.ClearAllEnemyExceptCurrent();
            _totalLevelNum.Value = _enemyProvider.TotalLevelNum;
        }
    }

    public struct EnemyCardWrapper : INetworkSerializeByMemcpy
    {
        public EnemyCard value;
    }
}