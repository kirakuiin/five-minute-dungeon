using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using GameLib.UI.SectorLayout;
using Unity.Netcode;
using UnityEngine;

namespace UI.Gameplay
{
    /// <summary>
    /// 平滑动画
    /// </summary>
    public class SmoothSectorAnimator : SectorAnimator
    {
        private readonly List<Transform> _playingObjs = new();
        
        public override void Play(Transform childTransform, Vector3 targetPosition, Quaternion targetRotation)
        {
            childTransform.DOMove(targetPosition, animateTime);
            childTransform.DORotateQuaternion(targetRotation, animateTime).OnComplete(() => ClearTransform(childTransform));
            _playingObjs.Add(childTransform);
        }

        private void ClearTransform(Transform trans)
        {
            _playingObjs.Remove(trans);
        }

        public override void Stop()
        {
            foreach (var trans in _playingObjs)
            {
                trans.DOKill();
            }
            _playingObjs.Clear();
        }
    }
}
