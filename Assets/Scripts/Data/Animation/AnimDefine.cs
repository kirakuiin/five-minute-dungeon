using System;
using System.Collections.Generic;

namespace Data.Animation
{
    [Serializable]
    public struct AnimContext
    {
        public AnimTarget source;

        public List<AnimTarget> targets;
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