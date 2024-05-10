using System.Linq;
using Data.Animation;
using GameLib.Common.Extension;
using Gameplay.Core;
using UI.Model;
using UnityEngine;
using UnityEngine.Assertions;

namespace Behave
{
    public class PositionInfo : MonoBehaviour, IPositionInfo
    {
        [SerializeField] private Transform enemiesCenter;

        [SerializeField] private Transform playersCenter;
        
        [SerializeField]
        private EnemyModelController enemiesController;

        [SerializeField]
        private PlayerModelController playerModelController;

        public Vector3 GetAnimTargetPos(AnimTarget target)
        {
            var obj = target.type switch
            {
                AnimTargetType.Enemy => enemiesController.GetModel(target.id),
                AnimTargetType.Player => playerModelController.GetModel(target.id),
                _ => null,
            };
            Assert.IsNotNull(obj, $"{target.type}不能为{AnimTargetType.Dungeon}");
            return obj.transform.position;
        }

        public Vector3 GetEnemyCenter()
        {
            return enemiesCenter.position;
        }

        public Vector3 GetPlayerCenter()
        {
            return playersCenter.position;
        }

        public Vector3 GetARandomNonEventEnemy()
        {
            var infos = GamePlayContext.Instance.GetLevelRuntimeInfo().GetAllEnemiesInfo();
            var ids = infos.Where(info=> !info.Value.IsEventCard()).Select(info => info.Key).ToList();
            if (ids.Count == 0) return GetEnemyCenter();
            var id = new System.Random().Choice(ids);
            return GetAnimTargetPos(new AnimTarget() { type = AnimTargetType.Enemy, id = id });
        }
    }
}