using System;
using System.Collections.Generic;
using GameLib.Common.Extension;
using PlasticPipe.PlasticProtocol.Messages;
using UnityEngine;
using Random = System.Random;

namespace Data
{
    [CreateAssetMenu(fileName = "AudioData", menuName = "数据/音效数据", order = 0)]
    public class AudioData : ScriptableObject
    {
        public List<AudioUnit> config;

        public AudioInfo Get(string audioName)
        {
            return config.Find(unit => unit.name == audioName).info;
        }
    }

    [Serializable]
    public struct AudioUnit
    {
        public string name;

        public AudioInfo info;
    }

    [Serializable]
    public struct AudioInfo
    {
        public List<AudioClip> clip;

        public AudioClip GetRandom()
        {
            var random = new Random();
            return random.Choice(clip);
        }
    }
}