using System.Linq;
using Cinemachine;
using GameLib.Common;
using GameLib.Common.Extension;
using UnityEngine;

namespace Gameplay.Camera
{
    /// <summary>
    /// 相机控制器。
    /// </summary>
    [RequireComponent(typeof(CinemachineClearShot))]
    public class CameraControl : MonoSingleton<CameraControl>
    {
        /// <summary>
        /// 相机优先级。
        /// </summary>
        private enum CameraPriority
        {
            Max = 20,
            Normal = 10,
        }
        
        [SerializeField] private CinemachineVirtualCamera mainCamera;

        [SerializeField] private CinemachineVirtualCamera enemyCamera;
        
        [SerializeField] private CinemachineVirtualCamera playerCamera;
        
        [SerializeField] private CinemachineVirtualCamera skillCamera;

        private CinemachineClearShot _clearShot;

        protected override void OnInitialized()
        {
            _clearShot = GetComponent<CinemachineClearShot>();
        }

        /// <summary>
        /// 获得当前激活相机。
        /// </summary>
        public CinemachineVirtualCamera ActiveCamera =>
            _clearShot.ChildCameras.First(c => c.Priority == (int)CameraPriority.Max) as CinemachineVirtualCamera;

        /// <summary>
        /// 激活主相机。
        /// </summary>
        public void ActiveMain()
        {
            ResetAllCamerasPriority();
            mainCamera.Priority = (int)CameraPriority.Max;
        }

        private void ResetAllCamerasPriority()
        {
            _clearShot.ChildCameras.Apply(c => c.Priority = (int)CameraPriority.Normal);
        }
        
        /// <summary>
        /// 激活玩家相机。
        /// </summary>
        public void ActivePlayerCamera()
        {
            ResetAllCamerasPriority();
            playerCamera.Priority = (int)CameraPriority.Max;
        }
        
        /// <summary>
        /// 激活敌方相机。
        /// </summary>
        public void ActiveEnemyCamera()
        {
            ResetAllCamerasPriority();
            enemyCamera.Priority = (int)CameraPriority.Max;
        }
        
        /// <summary>
        /// 激活技能相机。
        /// </summary>
        public void ActiveSkillCamera()
        {
            ResetAllCamerasPriority();
            skillCamera.Priority = (int)CameraPriority.Max;
        }
    }
}