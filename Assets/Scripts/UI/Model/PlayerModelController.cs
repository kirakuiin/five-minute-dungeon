using System.Collections.Generic;
using System.Linq;
using Data;
using Data.Animation;
using GameLib.Common;
using GameLib.Common.Extension;
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
        
        private readonly Dictionary<ulong, GameObject> _models = new();
        
        private readonly DisposableGroup _disposableGroup = new();

        private int _curPosIdx;

        /// <summary>
        /// 玩家模型。
        /// </summary>
        public IEnumerable<GameObject> PlayersModel => _models.Values;
        
        /// <summary>
        /// 根据ID获得模型。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public GameObject GetModel(ulong id)
        {
            return _models[id];
        }

        private void Start()
        {
            InitListen();
        }

        private void InitListen()
        {
            NetworkManager.Singleton.OnConnectionEvent += OnConnectionEvent;
            _disposableGroup.Add(state.GameplayState.Subscribe(OnStateChanged));
            GamePlayContext.Instance.GetTimeRuntimeInfo().OnTimeIsFlow += OnTimeIsFlow;
        }

        private void OnTimeIsFlow(bool isFlow)
        {
            PlayersModel.Select(obj => obj.GetComponent<IModelAnimPlayer>()).Apply(
                model => model.SetPlay(isFlow));
        }

        private void OnConnectionEvent(NetworkManager manager, ConnectionEventData data)
        {
            if (data.EventType == ConnectionEvent.ClientDisconnected && _models.ContainsKey(data.ClientId))
            {
                Destroy(_models[data.ClientId]);
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
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localRotation = Quaternion.identity;
                _curPosIdx += 1;
            }
        }
        
        private GameObject CreateClassModel(ulong clientID)
        {
            var classType = GamePlayContext.Instance.GetPlayerRuntimeInfo(clientID).PlayerClass;
            var prefab = DataService.Instance.GetClassData(classType).classPrefab;
            var obj = Instantiate(prefab);
            obj.GetComponent<PlayerModel>().Init(clientID);
            _models[clientID] = obj;
            return obj;
        }
    }
}