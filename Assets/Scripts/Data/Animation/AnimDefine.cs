using System;
using System.Collections.Generic;
using Unity.Netcode;

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
    public struct AnimTarget : INetworkSerializeByMemcpy
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

    public enum StillVfxTargetType
    {
        Source=0,
        Target=1,
        EnemyCenter=2,
    }
}