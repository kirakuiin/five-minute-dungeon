﻿using Data.Animation;
using UnityEngine;

namespace UI.Model
{
    /// <summary>
    /// 模型动画控制。
    /// </summary>
    public class ModelAnimController : MonoBehaviour, IModelAnimPlayer
    {
        [SerializeField] private Animator animator;
        
        private static readonly int Lose = Animator.StringToHash("Lose");
        
        private static readonly int Win = Animator.StringToHash("Win");

        public void PlayWin()
        {
            animator.SetTrigger(Win);
        }

        public void PlayLose()
        {
            animator.SetTrigger(Lose);
        }

        public void PlayAttack()
        {
            throw new System.NotImplementedException();
        }

        public void PlayCastSkill()
        {
            throw new System.NotImplementedException();
        }

        public void PlayHurt()
        {
            throw new System.NotImplementedException();
        }

        public void PlayDizzy()
        {
            throw new System.NotImplementedException();
        }

        public void PlayIdle()
        {
            throw new System.NotImplementedException();
        }
    }
}