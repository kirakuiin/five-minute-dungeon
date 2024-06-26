﻿using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Data.Check;
using Data.Instruction;
using GameLib.Common.DataStructure;
using GameLib.Common.Extension;
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

        protected override void OnSynchronize<T>(ref BufferSerializer<T> serializer)
        {
            if (serializer.IsWriter)
            {
                var writer = serializer.GetFastBufferWriter();
                WriteEnemyInfo(writer);
                WriteResPool(writer);
            }
            else if (serializer.IsReader)
            {
                var reader = serializer.GetFastBufferReader();
                ReadEnemyInfo(reader);
                ReadResPool(reader);
            }
        }
        
        private void WriteEnemyInfo(FastBufferWriter writer)
        {
            writer.WriteValueSafe(_enemyInfos.Count);
            foreach (var pair in _enemyInfos)
            {
                writer.WriteValueSafe(pair.Key);
                writer.WriteValueSafe(pair.Value);
            }
        }
        
        private void WriteResPool(FastBufferWriter writer)
        {
            writer.WriteValueSafe(_resPool.Count);
            foreach (var pair in _resPool)
            {
                writer.WriteValueSafe(pair.Key);
                writer.WriteValueSafe(pair.Value);
            }
        }

        private void ReadEnemyInfo(FastBufferReader reader)
        {
            reader.ReadValueSafe(out int count);
            Enumerable.Range(0, count).Apply(
                _ =>
                {
                    reader.ReadValueSafe(out ulong key);
                    reader.ReadValueSafe(out EnemyCard card);
                    _enemyInfos[key] = card;
                }
            );
        }

        private void ReadResPool(FastBufferReader reader)
        {
            reader.ReadValueSafe(out int count);
            Enumerable.Range(0, count).Apply(
                _ =>
                {
                    reader.ReadValueSafe(out Resource key);
                    reader.ReadValueSafe(out long num);
                    _resPool[key] = num;
                }
            );
        }

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
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void AddResourceClientRpc(Resource type, int num)
        {
            _resPool[type] += num;
            OnResourceAdded?.Invoke(type, num);
        }

        public void ProcessLevel()
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
            if (GetAllEnemiesInfo().Count == 1 && _enemyInfos.ContainsKey(enemyID))
            {
                ClearResourcePoolClientRpc();
            }
            DestroyEnemyClientRpc(enemyID);
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
        
        public bool IsComplete() => _enemyProvider.CurProgress >= _enemyProvider.TotalLevelNum && _enemyInfos.Count == 0;

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