using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    /// <summary>
    /// Boss数据。
    /// </summary>
    [CreateAssetMenu(fileName = "BossData", menuName = "数据/首领数据", order = 0)]
    public class BossData : DictionaryScriptObj<Resource, int>
    {
        [Tooltip("boss类别")]
        public Boss boss;

        [Tooltip("需要的门卡数量")]
        public int doorCardNum;

        [Tooltip("需要的挑战卡数量")]
        public int challengeNum;
    }
}