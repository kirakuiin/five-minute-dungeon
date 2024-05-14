using System.Collections;
using System.Collections.Generic;
using Data.Animation;
using DG.Tweening;
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

        private Vector3? originLocalRot;

        public void PlayAnim(string animName, float speed)
        {
            animators.Apply(animator => animator.SetTrigger(Animator.StringToHash(animName)));
            animators.Apply(animator => animator.speed = speed);
        }

        public void SetPlay(bool isPlay)
        {
            animators.Apply(animator => animator.enabled = isPlay);
        }

        public void ChangeTo(ModelChangeParam param)
        {
            originPos ??= body.position;
            originLocalRot ??= body.localRotation.eulerAngles;
            if (param.mode == ModelChangeMode.Instantly)
            {
                body.position = param.position;
                body.localRotation = Quaternion.Euler(param.localRot);
            }
            else
            {
                body.DOMove(param.position, param.changeTime);
                body.DOLocalRotate(param.localRot, param.changeTime);
            }
        }
        

        public void ChangeBack()
        {
            StartCoroutine(ChangeBackCoroutine());
        }

        private IEnumerator ChangeBackCoroutine()
        {
            if (originPos == null || originLocalRot == null) yield break;
            var rot = Quaternion.Euler(originLocalRot.Value);
            while (body.position != originPos.Value || body.localRotation != rot)
            {
                body.position = originPos.Value;
                body.localRotation = Quaternion.Euler(originLocalRot.Value);
                yield return null;
            }
        }
    }
}