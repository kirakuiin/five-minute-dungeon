using System;
using System.Collections.Generic;

namespace Data.Animation
{
    /// <summary>
    /// 动画参数。
    /// </summary>
    [Serializable]
    public struct AnimParam
    {
        /// <summary>
        /// 动画的释放主体。
        /// </summary>
        public AnimTarget source;

        /// <summary>
        /// 动画的目标。
        /// </summary>
        public List<AnimTarget> targetList;
    }

    /// <summary>
    /// 动画对象。
    /// </summary>
    [Serializable]
    public struct AnimTarget
    {
        public ulong id;
        
        public AnimTargetType type;
    }

    public enum AnimTargetType
    {
        Player=0,
        Enemy=1,
        Dungeon=2,
    }
}