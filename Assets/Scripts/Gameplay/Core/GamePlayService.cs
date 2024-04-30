using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using GameLib.Common;
using GameLib.Network.NGO;
using Gameplay.Core.State;
using Gameplay.GameState;
using Gameplay.Message;
using Unity.Netcode;
using UnityEngine;

namespace Gameplay.Core
{
    /// <summary>
    /// 玩法服务器，主要控制游戏玩法进程。
    /// </summary>
    public class GamePlayService : NetworkSingleton<GamePlayService>
    {
        [SerializeField] private GamePlayState gamePlay;
        
        private GameplayServiceState _curState;

        private PlayableChecker _checker;

        private readonly DisposableGroup _disposable = new();
        
        private readonly Dictionary<string, GameplayServiceState> _states = new();

        private GamePlayContext Context => GamePlayContext.Instance;

        /// <summary>
        /// 状态改变事件。
        /// </summary>
        public event Action<GameServiceStatus> OnStateChanged;

        /// <summary>
        /// 当前状态。
        /// </summary>
        private GameServiceStatus CurrentStatus { set; get; }

        /// <summary>
        /// 发布消息。(内部使用)
        /// </summary>
        /// <param name="status"></param>
        public void NotifyStatus(GameServiceStatus status)
        {
            NotifyStatusClientRpc(status);
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void NotifyStatusClientRpc(GameServiceStatus status)
        {
            CurrentStatus = status;
            OnStateChanged?.Invoke(status);
        }

        private void Start()
        {
            _disposable.Add(gamePlay.GameplayState.Subscribe(OnInitState));
        }
        
        private void OnInitState(GamePlayStateMsg msg)
        {
            if (msg.state != GamePlayStateEnum.InitDone) return;
            _checker = new PlayableChecker();
        }

        /// <summary>
        /// 启动服务。
        /// </summary>
        public async void StartService()
        {
            if (!IsServer) return;
            await ChangeState<BeginState>();
            InvokeRepeating(nameof(CheckExit), 0, 0.5f);
        }
        
        private async void CheckExit()
        {
            var haveTime = Context.GetTimeRuntimeInfo().RemainTime > 0;
            var haveCards = Context.GetAllClientIDs().Select(id => Context.GetPlayerRuntimeInfo(id).IsHaveCards)
                .Any(haveCards => haveCards);
            if (haveTime && haveCards) return;
            await ChangeState<EndState>();
        }

        /// <summary>
        /// 关闭服务。
        /// </summary>
        public void StopService()
        {
            if (!IsServer) return;
            CancelInvoke();
            gamePlay.GoToPostGame();
        }

        public async Task ChangeState<T>() where T : GameplayServiceState, new()
        {
            if (!IsServer) return;
            Debug.Log($"进入{typeof(T).Name}状态。");
            CreateStateIfNeeded<T>();

            if (_curState != null)
            {
                await _curState.Exit();
            }
            _curState = _states[GetKey<T>()];
            await _curState.Enter();
        }

        private string GetKey<T>() where T : GameplayServiceState, new()
        {
            return typeof(T).Name;
        }

        private void CreateStateIfNeeded<T>() where T : GameplayServiceState, new()
        {
            var key = GetKey<T>();
            if (_states.ContainsKey(key)) return;
            _states[key] = new T();
            _states[key].SetService(this);
        }
        
        /// <summary>
        /// 执行释放技能效果。
        /// </summary>
        /// <param name="skill"></param>
        public void CastSkill(Skill skill)
        {
            CastSkillServerRpc(skill);
        }

        [Rpc(SendTo.Server)]
        private void CastSkillServerRpc(Skill skill, RpcParams param=default)
        {
            if (!CanICastSkill(skill)) return;
            _curState.ExecuteAction(
                new GameAction
                {
                    graph = DataService.Instance.GetSkillData(skill).action,
                    clientID = param.Receive.SenderClientId,
                }
            );
        }

        /// <summary>
        /// 执行打出手牌效果。
        /// </summary>
        /// <param name="card"></param>
        public void PlayCard(Card card)
        {
            PlayCardServerRpc(card);
            Context.GetPlayerController().Play(card);
        }

        [Rpc(SendTo.Server)]
        private void PlayCardServerRpc(Card card, RpcParams param=default)
        {
            if (!CanIPlayThisCard(card)) return;
            Context.GetTimeController().Continue();
            _curState.ExecuteAction(
                new GameAction
                {
                    graph = DataService.Instance.GetPlayerCardData(card).action,
                    clientID = param.Receive.SenderClientId,
                }
            );
        }

        /// <summary>
        /// 是否可以释放法术？
        /// </summary>
        public bool CanICastSkill(Skill skill)
        {
            return IsActionPhase() && _checker.CheckSkill(skill);
        }

        private bool IsActionPhase()
        {
            var isRightState = CurrentStatus.state is ServiceState.EventResolve or ServiceState.ListenAction;
            return isRightState && !CurrentStatus.IsEventResolving;
        }

        /// <summary>
        /// 是否可以打出卡牌？
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public bool CanIPlayThisCard(Card card)
        {
            return IsActionPhase() && _checker.CheckCard(card);
        }
    }
}