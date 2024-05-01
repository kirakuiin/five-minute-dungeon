using System.Collections.Generic;
using Data;
using GameLib.Network.NGO;

namespace Gameplay.Data
{
    /// <summary>
    /// 一次团队挑战的结果。
    /// </summary>
    public class ChallengeResultData : BinaryNetworkPacket
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
        public List<Class> squadList;

        protected override byte[] WriteBytes()
        {
            var writer = GetJsonWriter();
            writer.Serialize(isWin);
            writer.Serialize(boss);
            writer.Serialize(useTime);
            writer.Serialize(reason);
            writer.Serialize(squadList);
            return writer.GetBytes();
        }

        protected override void ReadBytes(byte[] stream)
        {
            var reader = GetJsonReader(stream);
            reader.Deserialize(ref isWin);
            reader.Deserialize(ref boss);
            reader.Deserialize(ref useTime);
            reader.Deserialize(ref reason);
            reader.Deserialize(ref squadList);
        }
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