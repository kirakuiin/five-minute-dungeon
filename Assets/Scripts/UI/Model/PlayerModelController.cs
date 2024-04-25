using System.Collections.Generic;
using Data;
using GameLib.Common;
using Gameplay.Core;
using Gameplay.GameState;
using Gameplay.Message;
using Unity.Netcode;
using UnityEngine;

namespace UI.Model
{
    public class PlayerModelController : MonoBehaviour
    {
        [SerializeField] private List<Transform> posList;

        [SerializeField] private GamePlayState state;
        
        private readonly Dictionary<ulong, NetworkObject> _models = new();
        
        private readonly DisposableGroup _disposableGroup = new();

        private int _curPosIdx;


        private void Start()
        {
            if (!NetworkManager.Singleton.IsServer) return;
            InitListen();
        }

        private void InitListen()
        {
            NetworkManager.Singleton.OnConnectionEvent += OnConnectionEvent;
            _disposableGroup.Add(state.GameplayState.Subscribe(OnStateChanged));
        }

        private void OnConnectionEvent(NetworkManager manager, ConnectionEventData data)
        {
            if (data.EventType == ConnectionEvent.ClientDisconnected)
            {
                _models.Remove(data.ClientId);
            }
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
            foreach (var clientID in GamePlayContext.Instance.GetAllClientIDs())
            {
                var obj = CreateClassModel(clientID);
                obj.transform.SetParent(posList[_curPosIdx], false);
                _curPosIdx += 1;
            }
        }
        
        private NetworkObject CreateClassModel(ulong clientID)
        {
            var classType = GamePlayContext.Instance.GetPlayerRuntimeInfo(clientID).PlayerClass;
            var prefab = DataService.Instance.GetClassData(classType).classPrefab;
            var obj = NetworkObject.InstantiateAndSpawn(prefab, NetworkManager.Singleton, clientID);
            _models[clientID] = obj;
            return obj;
        }
    }
}