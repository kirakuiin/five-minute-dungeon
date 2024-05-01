using System.Threading.Tasks;
using Gameplay.Progress;

namespace Gameplay.Core.State
{
    /// <summary>
    /// 结束状态。
    /// </summary>
    public class EndState : GameplayServiceState
    {
        public override ServiceState State => ServiceState.End;

        private const int EndDelay = 500;
        
        public override async Task Enter()
        {
            UpdateStatus(GameServiceStatus.Create(State));
            GameProgress.Instance.GenerateChallengeResult();
            await Task.Delay(EndDelay);
            Service.StopService();
        }
    }
}