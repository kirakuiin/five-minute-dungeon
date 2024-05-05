using System.Threading.Tasks;
using Data;
using Data.Check;
using Data.Instruction;
using Unity.Netcode;

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
            OnActionBegin += OnBegin;
            OnActionDone += OnDone;
            _levelRuntime.OnEnemyDestroyed += OnEnemyDestroyed;
            Context.GetTimeRuntimeInfo().OnTimeUpdated += OnTimeUpdate;
        }

        public override ServiceState State => ServiceState.EventResolve;

        public override async Task Enter()
        {
            UpdateStatus(GameServiceStatus.Create(State));
            _countdown = GameRule.EventCancelWaitTime;
            StartActionCycle();
            await Task.CompletedTask;
        }
        
        private void OnBegin(GameAction action)
        {
            if (Service.Status.CurrentStatus.state != ServiceState.EventResolve) return;
            if (action is EventAction)
            {
                UpdateStatus(GameServiceStatus.Create(State, ServiceStage.Resolving));
            }
        }

        private async void OnDone(GameAction action)
        {
            if (Service.Status.CurrentStatus.state != ServiceState.EventResolve) return;
            if (action is EventAction e)
            {
                _levelController.DestroyEnemyCard(e.subjectID);
                UpdateStatus(GameServiceStatus.Create(State));
                await ChangeState<RevealEnemyState>();
            }
        }
        
        private async void OnEnemyDestroyed(EnemyChangeEvent e)
        {
            if (Service.Status.CurrentStatus.state != ServiceState.EventResolve) return;
            if (_levelRuntime.GetAllEnemiesInfo().Count == 0)
            {
                await ChangeState<RevealEnemyState>();
            }
        }

        public override async Task Exit()
        {
            await StopActionCycle();
        }

        private void OnTimeUpdate(int t)
        {
            if (Service.Status.CurrentStatus.state != ServiceState.EventResolve) return;
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
                if (!enemyInfo.IsEventCard()) continue;
                var action = new EventAction()
                {
                    clientID = NetworkManager.Singleton.LocalClientId,
                    graph = DataService.Instance.GetChallengeCardData((Challenge)pair.Value.value).action,
                    subjectID = pair.Key,
                };
                ExecuteAction(action);
            }
        }
    }
}