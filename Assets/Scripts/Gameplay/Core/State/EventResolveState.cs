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
        
        public override async Task Enter()
        {
            _countdown = GameRule.EventCancelWaitTime;
            onActionDone += OnActionDone;
            StartActionCycle();
            await Task.CompletedTask;
        }

        private void OnActionDone(GameAction action)
        {
            if (action is EventAction e)
            {
                _levelController.DestroyEnemyCard(e.enemyID);
            }
        }

        public override async Task Exit()
        {
            await StopActionCycle();
            onActionDone -= OnActionDone;
        }

        public override async void Update()
        {
            _countdown -= Time.deltaTime;
            if (_countdown <= 0)
            {
                _countdown = float.MaxValue;
                ExecuteEvent();
            }

            if (!_levelRuntime.IsContainEvent())
            {
                _countdown = float.MaxValue;
                await ChangeState<ListenActionState>();
            }
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