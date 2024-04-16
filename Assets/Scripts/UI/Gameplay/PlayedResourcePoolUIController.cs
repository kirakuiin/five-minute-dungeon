using Data;
using UnityEngine;

namespace UI.Gameplay
{
    /// <summary>
    /// 击败敌人所需资源池。
    /// </summary>
    public class PlayedResourcePoolUIController : ResourcePoolUIController
    {
        protected override void OnResourceAdded(Resource res, int num)
        {
            for (var _ = 0; _ < num; _++)
            {
                CreateIcon(res);
            }
        }

        /// <summary>
        /// 刷新UI。
        /// </summary>
        public void RefreshUI()
        {
            CleanAll();
            foreach (var resource in LevelInfo.GetAlreadyPlayedResources())
            {
                CreateIcon(resource);
            }
        }
    }
}