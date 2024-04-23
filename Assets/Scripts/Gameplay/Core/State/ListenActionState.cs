using System.Threading.Tasks;
using Data.Check;

namespace Gameplay.Core.State
{
    public class ListenActionState : ActionState
    {
        private readonly ILevelRuntimeInfo _info;

        public ListenActionState()
        {
            _info = Context.GetLevelRuntimeInfo();
        }

        public override ServiceState State => ServiceState.ListenAction;

        public override async Task Enter()
        {
            UpdateStatus(GameServiceStatus.Create(State));
            _info.OnEnemyDestroyed += OnEnemyDestroyed;
            StartActionCycle();
            await Task.CompletedTask;
        }

        private async void OnEnemyDestroyed(EnemyChangeEvent e)
        {
            if (_info.GetAllEnemiesInfo().Count == 0)
            {
                await ChangeState<RevealEnemyState>();
            }
        }

        public override async Task Exit()
        {
            await StopActionCycle();
            _info.OnEnemyDestroyed -= OnEnemyDestroyed;
        }
    }
}