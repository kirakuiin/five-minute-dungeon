﻿using Data.Animation;
using UnityEngine;

namespace Behave
{
    /// <summary>
    /// 表现控制器。
    /// </summary>
    public class BehaveController : MonoBehaviour, IBehaveController
    {
        [SerializeField] private ModelAnimPlayer modelPlayer;

        [SerializeField] private PositionInfo positionInfo;

        [SerializeField] private VfxPlayer vfxPlayer;

        public IModelAnimPlayer GetModelPlayer(AnimTarget target)
        {
            modelPlayer.SetAnimTarget(target);
            return modelPlayer;
        }

        public IPositionInfo GetPositionInfo()
        {
            return positionInfo;
        }

        public IVfxPlayer GetVfxPlayer()
        {
            return vfxPlayer;
        }
    }
}