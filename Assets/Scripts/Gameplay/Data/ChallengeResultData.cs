using System.Collections.Generic;
using Data;
using Unity.Netcode;

namespace Gameplay.Data
{
    /// <summary>
    /// 一次团队挑战的结果。
    /// </summary>
    public struct ChallengeResultData : INetworkSerializeByMemcpy
    {
        /// <summary>
        /// 是否胜利？
        /// </summary>
        public bool isWin;

        /// <summary>
        /// 挑战的boss
        /// </summary>
        public Boss boss;

        /// <summary>
        /// 挑战时长。
        /// </summary>
        public int useTime;

        /// <summary>
        /// 失败原因。
        /// </summary>
        public FailureReason reason;

        /// <summary>
        /// 小队成员。
        /// </summary>
        public Class[] squadList;
    }

    /// <summary>
    /// 失败原因。
    /// </summary>
    public enum FailureReason
    {
        Timeout,
        CardExhausted,
    }
}