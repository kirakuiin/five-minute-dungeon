using Audio;
using Data.Animation;
using GameLib.Common.Extension;
using UI.Model;
using Unity.Netcode;
using UnityEngine;

namespace Behave
{
    /// <summary>
    /// 音效播放器。
    /// </summary>
    public class AudioEffectPlayer : NetworkBehaviour, IAudioEffectPlayer
    {
        [SerializeField]
        private EnemyModelController enemiesController;

        [SerializeField]
        private PlayerModelController playerModelController;

        [SerializeField]
        private SoundEffectPlayer globalPlayer;

        public void PlayGlobal(AudioParam param)
        {
            PlayGlobalRpc(param);
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void PlayGlobalRpc(AudioParam param)
        {
            globalPlayer.Play(param);
        }

        public void PlayOnTarget(AnimTarget target, AudioParam param)
        {
            PlayOnTargetRpc(target, param);
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void PlayOnTargetRpc(AnimTarget target, AudioParam param)
        {
            if (target.type == AnimTargetType.Dungeon) return;
            var objs = target.type == AnimTargetType.Player
                ? playerModelController.PlayersModel
                : enemiesController.EnemiesModel;
            objs.Apply(obj => obj.GetComponent<SoundEffectPlayer>().Play(param));
        }
    }
}