using System.Collections.Generic;
using System.Linq;
using Data.Animation;
using UI.Model;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

namespace Behave
{
    /// <summary>
    /// 表现控制器。
    /// </summary>
    public class BehaveController : MonoBehaviour, IBehaveController
    {
        [SerializeField]
        private EnemyModelController enemiesController;

        [SerializeField]
        private PlayerModelController playerModelController;

        [SerializeField] private PositionInfo positionInfo;

        [FormerlySerializedAs("fbxPlayer")] [SerializeField] private VfxPlayer vfxPlayer;

        public IModelAnimPlayer GetModelPlayer(AnimTarget target)
        {
            var obj = target.type switch
            {
                AnimTargetType.Enemy => enemiesController.GetModel(target.id),
                AnimTargetType.Player => playerModelController.GetModel(target.id),
                _ => null,
            };
            Assert.IsNotNull(obj, $"{target.type}不能为{AnimTargetType.Dungeon}");
            return obj.GetComponent<IModelAnimPlayer>();
        }

        public IPositionInfo GetPositionInfo()
        {
            return positionInfo;
        }

        public IVfxPlayer GetVfxPlayer()
        {
            return vfxPlayer;
        }
    }
}