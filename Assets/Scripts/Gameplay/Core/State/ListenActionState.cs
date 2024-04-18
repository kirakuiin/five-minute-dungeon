using System.Threading.Tasks;
using Data;

namespace Gameplay.Core.State
{
    public class ListenActionState : ActionState
    {
        private readonly ILevelRuntimeInfo _info;

        public ListenActionState()
        {
            _info = Context.GetLevelRuntimeInfo();
        }
        
        public override async Task Enter()
        {
            _info.OnEnemyDestroyed += OnEnemyDestroyed;
            StartActionCycle();
            await Task.CompletedTask;
        }

        private async void OnEnemyDestroyed(EnemyChangeEvent e)
        {
            if (_info.GetAllEnemyInfos().Count == 0)
            {
                await ChangeState<RevealEnemyState>();
            }
        }

        public override async Task Exit()
        {
            _info.OnEnemyDestroyed -= OnEnemyDestroyed;
            await StopActionCycle();
        }
    }
}