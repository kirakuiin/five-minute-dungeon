using System.Collections.Generic;
using Data.Animation;
using GameLib.Common.Extension;
using UnityEngine;

namespace UI.Model
{
    /// <summary>
    /// 模型动画控制。
    /// </summary>
    public class ModelAnimController : MonoBehaviour, IModelAnimPlayer
    {
        [SerializeField] private List<Animator> animators;

        [SerializeField] private Transform body;

        private Vector3? originPos;
        
        private static readonly int Lose = Animator.StringToHash("Lose");
        private static readonly int Win = Animator.StringToHash("Win");
        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int Hurt = Animator.StringToHash("Hurt");
        private static readonly int Cast = Animator.StringToHash("Cast");
        private static readonly int Dizzy = Animator.StringToHash("Dizzy");
        private static readonly int Attack = Animator.StringToHash("Attack");

        public void PlayWin()
        {
            animators.Apply(animator => animator.SetTrigger(Win));
        }

        public void PlayLose()
        {
            animators.Apply(animator => animator.SetTrigger(Lose));
        }

        public void PlayAttack()
        {
            animators.Apply(animator => animator.SetTrigger(Attack));
        }

        public void PlayCastSkill()
        {
            animators.Apply(animator => animator.SetTrigger(Cast));
        }

        public void PlayHurt()
        {
            animators.Apply(animator => animator.SetTrigger(Hurt));
        }

        public void PlayDizzy()
        {
            animators.Apply(animator => animator.SetTrigger(Dizzy));
        }

        public void PlayIdle()
        {
            animators.Apply(animator => animator.SetTrigger(Idle));
        }

        public void SetPlay(bool isPlay)
        {
            animators.Apply(animator => animator.enabled = isPlay);
        }

        public void MoveTo(Vector3 position)
        {
            originPos ??= body.position;
            body.position = position;
        }

        public void MoveBack()
        {
            if (originPos != null)
            {
                body.position = originPos.Value;
            }
        }
    }
}