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
        
        public override Task Enter()
        {
            UpdateStatus(GameServiceStatus.Create(State));
            GameProgress.Instance.GenerateChallengeResult();
            Service.StopService();
            return Task.CompletedTask;
        }
    }
}