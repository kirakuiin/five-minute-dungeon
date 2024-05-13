using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "VfxData", menuName = "数据/特效数据", order = 0)]
    public class VfxData : ScriptableObject
    {
        public List<VfxUnit> config;

        public VfxInfo Get(string vfxName)
        {
            return config.Find(unit => unit.name == vfxName).info;
        }
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

    [Serializable]
    public struct VfxUnit
    {
        public string name;
        
        public VfxInfo info;
    }
}