﻿using System.Collections.Generic;
using System.Linq;
using Data;
using Data.Animation;
using Data.Check;
using GameLib.Common;
using GameLib.Common.Extension;
using Gameplay.Core;
using Gameplay.GameState;
using Gameplay.Message;
using UnityEngine;

namespace UI.Model
{
    /// <summary>
    /// 敌方模型控制器。
    /// </summary>
    public class EnemyModelController : MonoBehaviour
    {
        [SerializeField] private List<Transform> posList;
        
        [SerializeField] private Transform bossPos;

        [SerializeField] private GamePlayState state;
        
        private static readonly HashSet<int> AllIdx = new() {0, 1, 2};
        
        private readonly Dictionary<ulong, GameObject> _models = new();

        private readonly Dictionary<ulong, int> _occupiedIdx = new();

        private readonly DisposableGroup _disposableGroup = new();

        /// <summary>
        /// 获得全部敌方模型。
        /// </summary>
        public IEnumerable<GameObject> EnemiesModel => _models.Values;

        /// <summary>
        /// 根据ID获得模型。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public GameObject GetModel(ulong id)
        {
            return _models[id];
        }
        
        private ILevelRuntimeInfo LevelInfo => GamePlayContext.Instance.GetLevelRuntimeInfo();
        
        private void Start()
        {
            InitListen();
        }

        private void InitListen()
        {
            _disposableGroup.Add(state.GameplayState.Subscribe(OnStateChanged));
            LevelInfo.OnEnemyDestroyed += OnEnemyDestroyed;
            LevelInfo.OnEnemyAdded += OnEnemyAdded;
            LevelInfo.OnResourceAdded += OnResAdded;
            GamePlayContext.Instance.GetTimeRuntimeInfo().OnTimeIsFlow += OnTimeIsFlow;
        }

        private void OnTimeIsFlow(bool isFlow)
        {
            EnemiesModel.Select(obj => obj.GetComponent<IModelAnimPlayer>()).Apply(
                model => model.SetPlay(isFlow));
        }

        private void OnStateChanged(GamePlayStateMsg msg)
        {
            if (msg.state == GamePlayStateEnum.InitDone)
            {
                Init();
            }
        }
        
        private void Init()
        {
            foreach (var (enemyID, enemyCard) in LevelInfo.GetAllEnemiesInfo())
            {
                SetMonsterModel(enemyID, enemyCard);
            }
        }
        
        private void SetMonsterModel(ulong enemyID, EnemyCard enemyCard)
        {
            var obj = CreateMonsterModel(enemyID, enemyCard);
            if (enemyCard.IsBossCard())
            {
                obj.transform.SetParent(bossPos, false);
            }
            else
            {
                var idx = GetIdleIdx();
                obj.transform.SetParent(posList[idx], false);
                _occupiedIdx[enemyID] = idx;
            }
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
            obj.GetComponent<EnemyModel>().Init(enemyID, enemyCard);
        }
        
        private GameObject CreateMonsterModel(ulong enemyID, EnemyCard card)
        { 
            var prefab = DataService.Instance.GetEnemyCardData(card).prefab;
            var obj = GameObjectPool.Instance.Get(prefab);
            _models[enemyID] = obj;
            return obj;
        }

        private int GetIdleIdx()
        {
            return AllIdx.Except(_occupiedIdx.Values).First();
        }
        
        private void OnEnemyAdded(EnemyChangeEvent @event)
        {
            SetMonsterModel(@event.enemyID, @event.enemyCard);
        }

        private void OnResAdded(Resource res, int num)
        {
            EnemiesModel.Apply(model => model.GetComponent<IModelAnimPlayer>().PlayHurt());
        }
        
        private void OnEnemyDestroyed(EnemyChangeEvent @event)
        {
            if (!@event.enemyCard.IsBossCard())
            {
                _occupiedIdx.Remove(@event.enemyID);
            }
            var prefab = DataService.Instance.GetEnemyCardData(@event.enemyCard).prefab;
            GameObjectPool.Instance.ReturnWithReParent(_models[@event.enemyID], prefab);
            _models.Remove(@event.enemyID);
        }
    }
}