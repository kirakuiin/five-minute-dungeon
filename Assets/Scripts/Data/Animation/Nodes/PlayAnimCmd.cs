using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameLib.Common.Extension;

namespace Data.Animation.Nodes
{
    /// <summary>
    /// 模型动画。
    /// </summary>
    public class PlayAnimCmd: AnimationBase
    {
        public string animName;

        public float speed = 1.0f;

        public bool isSource = true;
        
        public override async Task Execute(IBehaveController controller, AnimContext animContext)
        {
            var players = isSource ? new List<IModelAnimPlayer>() { controller.GetModelPlayer(animContext.source) }
                : animContext.targets.Select(controller.GetModelPlayer).ToList();
            players.Apply(player => player.PlayAnim(animName, speed));
            await Task.CompletedTask;
        }
    }
}