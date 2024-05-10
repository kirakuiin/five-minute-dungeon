using System;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "VfxData", menuName = "数据/特效数据", order = 0)]
    public class VfxData : DictionaryScriptObj<string, VfxInfo>
    {
    }

    /// <summary>
    /// Vfx配置信息。
    /// </summary>
    [Serializable]
    public struct VfxInfo
    {
        public GameObject prefab;

        public bool isParticleSystem;
    }
}