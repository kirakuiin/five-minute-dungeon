using System.Collections.Generic;
using System.Linq;
using Data;
using Data.Check;
using GameLib.Common;
using Gameplay.Core;
using Gameplay.GameState;
using Gameplay.Message;
using Unity.Netcode;
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
        
        private static readonly HashSet<int> AllIdx = new() {0, 1};
        
        private readonly Dictionary<ulong, NetworkObject> _models = new();

        private readonly Dictionary<ulong, int> _occupiedIdx = new();

        private readonly DisposableGroup _disposableGroup = new();
        
        private ILevelRuntimeInfo LevelInfo => GamePlayContext.Instance.GetLevelRuntimeInfo();
        
        private void Start()
        {
            if (!NetworkManager.Singleton.IsServer) return;
            InitListen();
        }

        private void InitListen()
        {
            _disposableGroup.Add(state.GameplayState.Subscribe(OnStateChanged));
            LevelInfo.OnEnemyDestroyed += OnEnemyDestroyed;
            LevelInfo.OnEnemyAdded += OnEnemyAdded;
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
        }
        
        private NetworkObject CreateMonsterModel(ulong enemyID, EnemyCard card)
        { 
            var prefab = DataService.Instance.GetEnemyCardData(card).prefab;
            var obj = NetworkObject.InstantiateAndSpawn(prefab, NetworkManager.Singleton, NetworkManager.Singleton.LocalClientId);
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
        
        private void OnEnemyDestroyed(EnemyChangeEvent @event)
        {
            if (!@event.enemyCard.IsBossCard())
            {
                _occupiedIdx.Remove(@event.enemyID);
            }
            _models[@event.enemyID].Despawn();
            _models.Remove(@event.enemyID);
        }
        

    }
}