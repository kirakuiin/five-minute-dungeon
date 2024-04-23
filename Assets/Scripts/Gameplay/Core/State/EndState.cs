using System.Threading.Tasks;

namespace Gameplay.Core.State
{
    public class EndState : GameplayServiceState
    {
        public override ServiceState State => ServiceState.End;
        
        public override Task Enter()
        {
            UpdateStatus(GameServiceStatus.Create(State));
            return base.Enter();
        }
    }
}