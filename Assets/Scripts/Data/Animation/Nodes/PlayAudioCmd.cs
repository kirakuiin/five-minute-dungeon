using System.Linq;
using System.Threading.Tasks;
using GameLib.Common.Extension;

namespace Data.Animation.Nodes
{
    public class PlayAudioCmd : AnimationBase
    {
        public string clipName;

        public float volume = 1.0f;
        
        public bool isOnSource = true;

        public bool isGlobal = false;
        
        public override async Task Execute(IBehaveController controller, AnimContext animContext)
        {
            var audioParam = new AudioParam { clipName = clipName, volume = volume };
            var player = controller.GetAudioPlayer();
            if (isGlobal)
            {
                player.PlayGlobal(audioParam);
                return;
            }
            if (isOnSource)
            {
                player.PlayOnTarget(animContext.source, audioParam);
            }
            else
            {
                animContext.targets.Apply(target => player.PlayOnTarget(target, audioParam));
            }

            await Task.CompletedTask;
        }
    }
}