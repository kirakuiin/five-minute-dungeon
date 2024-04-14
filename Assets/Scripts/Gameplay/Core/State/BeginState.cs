using System.Threading.Tasks;

namespace Gameplay.Core.State
{
    public class BeginState : GameplayServiceState
    {
        public override async Task Enter()
        {
            Context.GetTimeController().StartTimer(GameRule.CountdownTime);
            await ChangeState<RevealEnemyState>();
        }
    }
}