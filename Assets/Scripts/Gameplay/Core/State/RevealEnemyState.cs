using System.Linq;
using System.Threading.Tasks;
using Data;

namespace Gameplay.Core.State
{
    public class RevealEnemyState : GameplayServiceState
    {
        public override ServiceState State => ServiceState.RevealEnemy;

        public override async Task Enter()
        {
            UpdateStatus(GameServiceStatus.Create(State));
            var level = Context.GetLevelController();
            if (level.GetAllEnemiesInfo().Count > 0)
            {
                await ToNextState();
            }
            else if (level.IsComplete())
            {
                await ChangeState<EndState>();
            }
            else
            {
                if (level.IsReachBoss())
                {
                    level.RevealBoss();
                }
                else
                {
                    level.RevealNextLevel(1);
                }

                await ToNextState();
            }
        }

        private async Task ToNextState()
        {
            if (Context.GetLevelRuntimeInfo().IsContainEvent())
            {
                await ChangeState<EventResolveState>();
            }
            else
            {
                await ChangeState<ListenActionState>();
            }
        }
    }
}