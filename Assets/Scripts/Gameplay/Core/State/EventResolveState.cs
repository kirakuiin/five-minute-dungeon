using System.Threading.Tasks;
using Data;
using Data.Check;
using Data.Instruction;
using Unity.Netcode;
using UnityEngine;

namespace Gameplay.Core.State
{
    /// <summary>
    /// 结算事件阶段。
    /// </summary>
    public class EventResolveState : ActionState
    {
        private readonly ILevelRuntimeInfo _levelRuntime;

        private readonly ILevelController _levelController;

        private float _countdown;
        
        public EventResolveState()
        {
            _levelRuntime = Context.GetLevelRuntimeInfo();
            _levelController = Context.GetLevelController();
        }

        public override ServiceState State => ServiceState.EventResolve;

        public override async Task Enter()
        {
            UpdateStatus(GameServiceStatus.Create(State));
            _countdown = GameRule.EventCancelWaitTime;
            OnActionDone += OnDone;
            OnActionBegin += OnBegin;
            _levelRuntime.OnEnemyDestroyed += OnEnemyDestroyed;
            Context.GetTimeRuntimeInfo().OnTimeUpdated += OnTimeUpdate;
            StartActionCycle();
            await Task.CompletedTask;
        }
        
        private void OnBegin(GameAction action)
        {
            if (action is EventAction)
            {
                UpdateStatus(GameServiceStatus.Create(State, ServiceStage.Resolving));
            }
        }

        private async void OnDone(GameAction action)
        {
            if (action is EventAction e)
            {
                _levelController.DestroyEnemyCard(e.enemyID);
                UpdateStatus(GameServiceStatus.Create(State));
                await ChangeState<RevealEnemyState>();
            }
        }
        
        private async void OnEnemyDestroyed(EnemyChangeEvent e)
        {
            if (_levelRuntime.GetAllEnemiesInfo().Count == 0)
            {
                await ChangeState<RevealEnemyState>();
            }
        }

        public override async Task Exit()
        {
            await StopActionCycle();
            _levelRuntime.OnEnemyDestroyed -= OnEnemyDestroyed;
            Context.GetTimeRuntimeInfo().OnTimeUpdated -= OnTimeUpdate;
            OnActionDone -= OnDone;
            OnActionBegin -= OnBegin;
        }

        private void OnTimeUpdate(int t)
        {
            if (_countdown == 0)
            {
                ExecuteEvent();
            }
            _countdown -= 1;
        }

        private void ExecuteEvent()
        {
            foreach (var pair in _levelRuntime.GetAllEnemiesInfo())
            {
                var enemyInfo = pair.Value;
                if (!enemyInfo.IsEventCard()) return;
                var action = new EventAction()
                {
                    clientID = NetworkManager.Singleton.LocalClientId,
                    graph = DataService.Instance.GetChallengeCardData((Challenge)pair.Value.value).action,
                    enemyID = pair.Key,
                };
                ExecuteAction(action);
            }
        }
    }
}