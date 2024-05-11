using Data.Animation;
using UI.Model;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

namespace Behave
{
    /// <summary>
    /// 模型动画控制器。
    /// </summary>
    public class ModelAnimPlayer : NetworkBehaviour, IModelAnimPlayer
    {
        [SerializeField]
        private EnemyModelController enemiesController;

        [SerializeField]
        private PlayerModelController playerModelController;
        
        private IModelAnimPlayer _delegate;
        
        public void SetAnimTarget(AnimTarget target)
        {
            SetAnimTargetRpc(target);
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void SetAnimTargetRpc(AnimTarget target)
        {
            var obj = target.type switch
            {
                AnimTargetType.Enemy => enemiesController.GetModel(target.id),
                AnimTargetType.Player => playerModelController.GetModel(target.id),
                _ => null,
            };
            Assert.IsNotNull(obj, $"{target.type}不能为{AnimTargetType.Dungeon}");
            _delegate = obj.GetComponent<IModelAnimPlayer>();
        }

        public void PlayWin()
        {
            PlayWinRpc();
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void PlayWinRpc()
        {
            _delegate.PlayWin();
        }

        public void PlayLose()
        {
            PlayLoseRpc();
        }
        
        [Rpc(SendTo.ClientsAndHost)]
        private void PlayLoseRpc()
        {
            _delegate.PlayLose();
        }

        public void PlayAttack()
        {
            PlayAttackRpc();
        }
        
        [Rpc(SendTo.ClientsAndHost)]
        private void PlayAttackRpc()
        {
            _delegate.PlayAttack();
        }

        public void PlayCastSkill()
        {
            PlayCastSkillRpc();
        }
        
        [Rpc(SendTo.ClientsAndHost)]
        private void PlayCastSkillRpc()
        {
            _delegate.PlayCastSkill();
        }

        public void PlayHurt()
        {
            PlayHurtRpc();
        }
        
        [Rpc(SendTo.ClientsAndHost)]
        private void PlayHurtRpc()
        {
            _delegate.PlayHurt();
        }

        public void PlayDizzy()
        {
            PlayDizzyRpc();
        }
        
        [Rpc(SendTo.ClientsAndHost)]
        private void PlayDizzyRpc()
        {
            _delegate.PlayDizzy();
        }

        public void PlayIdle()
        {
            PlayIdleRpc();
        }
        
        [Rpc(SendTo.ClientsAndHost)]
        private void PlayIdleRpc()
        {
            _delegate.PlayIdle();
        }

        public void SetPlay(bool isPlay)
        {
            SetPlayRpc(isPlay);
        }
        
        [Rpc(SendTo.ClientsAndHost)]
        private void SetPlayRpc(bool isPlay)
        {
            _delegate.SetPlay(isPlay);
        }

        public void ChangeTo(ModelChangeParam param)
        {
            MoveToRpc(param);
        }
        
        [Rpc(SendTo.ClientsAndHost)]
        private void MoveToRpc(ModelChangeParam param)
        {
            _delegate.ChangeTo(param);
        }

        public void ChangeBack()
        {
            MoveBackRpc();
        }
        
        [Rpc(SendTo.ClientsAndHost)]
        private void MoveBackRpc()
        {
            _delegate.ChangeBack();
        }
    }
}