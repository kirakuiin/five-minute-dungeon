using System.Threading.Tasks;

namespace Gameplay.Core.State
{
    public class ListenActionState : ActionState
    {
        private bool _alreadyStop;

        private readonly ILevelRuntimeInfo _info;

        public ListenActionState()
        {
            _info = Context.GetLevelRuntimeInfo();
        }
        
        public override async Task Enter()
        {
            StartActionCycle();
            _alreadyStop = false;
            await Task.CompletedTask;
        }

        public override async Task Exit()
        {
            await StopActionCycle();
        }

        public override async void Update()
        {
            if (_info.GetAllEnemyInfos().Count == 0 && !_alreadyStop)
            {
                _alreadyStop = true;
                await ChangeState<RevealEnemyState>();
            }
        }
    }
}