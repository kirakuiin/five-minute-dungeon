using System;

namespace Protocol
{
    /// <summary>
    /// 连接携带数据。
    /// </summary>
    [Serializable]
    public struct ConnectionPayload
    {
        public string playerGuid;
        public string password;
    }
}