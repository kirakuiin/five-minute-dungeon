using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Data.Animation
{
    public struct AnimContext
    {
        public AnimTarget source;

        public List<AnimTarget> targets;

        public OtherAnimInfo other;
    }

    /// <summary>
    /// 非必要动画信息。
    /// </summary>
    public struct OtherAnimInfo
    {
        public Resource selectedRes;
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
        PlayerCenter=3,
        Center=4,
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

    [Serializable]
    public struct AudioParam : INetworkSerializeByMemcpy
    {
        public string clipName;
        public float volume;
    }

    /// <summary>
    /// 动作名称定义。
    /// </summary>
    public static class AnimNameDefine
    {
         public static readonly string Lose = "Lose";
         public static readonly string Win = "Win";
         public static readonly string Idle = "Idle";
         public static readonly string Hurt = "Hurt";
         public static readonly string Cast = "Cast";
         public static readonly string Dizzy = "Dizzy";
         public static readonly string Attack = "Attack";
         public static readonly string Jump = "Jump";
         public static readonly string Sprint = "Sprint";
    }

    /// <summary>
    /// 音效名称定义。
    /// </summary>
    public static class AudioEffectDefine
    {
    }
}