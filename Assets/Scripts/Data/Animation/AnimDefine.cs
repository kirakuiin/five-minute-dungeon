using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

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

    /// <summary>
    /// 静止vfx参数。
    /// </summary>
    [Serializable]
    public struct StillVfxParam : INetworkSerializeByMemcpy
    {
        public Vector3 target;
        public Vector3 rotation;
        public float duration;
        public float speed;
        public bool needAwait;
    }

    public enum ModelChangeMode
    {
        Instantly=0,
        LinerInterpolation=1,
    }

    /// <summary>
    /// 模型变化参数
    /// </summary>
    [Serializable]
    public struct ModelChangeParam : INetworkSerializeByMemcpy
    {
        public Vector3 position;
        public Vector3 localRot;
        public float changeTime;
        public ModelChangeMode mode;
    }
}