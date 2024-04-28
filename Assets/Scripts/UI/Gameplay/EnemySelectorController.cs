using Data;
using Data.Check;
using GameLib.Common.Extension;
using Gameplay.Camera;
using Gameplay.Core;
using UI.Model;
using UnityEngine;

namespace UI.Gameplay
{
    /// <summary>
    /// 敌人选择器。
    /// </summary>
    public class EnemySelectorController : MonoBehaviour
    {
        [SerializeField] private EnemyModelController modelController;
            
        private IEnemySelector Selector { set; get; }
        
        /// <summary>
        /// 初始化。
        /// </summary>
        public void Init()
        {
            Selector = GamePlayContext.Instance.GetPlayerRuntimeInfo().GetRuntimeInteractive().GetEnemySelector();
            Selector.OnEnemySelecting += OnEnemySelecting;
        }

        private void OnEnemySelecting(EnemyCardType type)
        {
            modelController.EnemiesModel.Apply(
                model => model.GetComponent<EnemyModelSelector>().EnterSelectMode(type, SetSelectEnemy)
            );
            CameraControl.Instance.ActiveEnemyCamera();
        }

        private void SetSelectEnemy(ulong enemyID)
        {
            modelController.EnemiesModel.Apply(obj => obj.GetComponent<EnemyModelSelector>().ExitSelectMode());
            Selector.SelectEnemy(enemyID);
            CameraControl.Instance.ActiveMain();
        }
    }
}