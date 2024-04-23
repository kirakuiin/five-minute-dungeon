using System.Threading.Tasks;

namespace Gameplay.Core.State
{
    public class BeginState : GameplayServiceState
    {
        public override ServiceState State => ServiceState.Begin;

        public override async Task Enter()
        {
            UpdateStatus(GameServiceStatus.Create(State));
            Context.GetTimeController().StartTimer(GameRule.CountdownTime);
            await ChangeState<RevealEnemyState>();
        }
    }
}