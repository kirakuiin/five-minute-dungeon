using System.Linq;
using Data;
using Gameplay.Core;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GM
{
    /// <summary>
    /// gm管理器。
    /// </summary>
    public class GmManager : MonoBehaviour
    {
        [SerializeField] private Transform dialogParent;

        [SerializeField] private GameObject uiPrefab;

        [SerializeField] private LevelController controller;

        private GamePlayContext Context => GamePlayContext.Instance;

        private GameObject _uiObj;
        
        public void OnTriggerShow(InputAction.CallbackContext callback)
        {
            if (!NetworkManager.Singleton.IsServer) return;
            if (callback.phase != InputActionPhase.Performed) return;
            if (_uiObj is null)
            {
                _uiObj = Instantiate(uiPrefab, dialogParent);
            }
            else
            {
                _uiObj.SetActive(!_uiObj.activeSelf);
            }
        }
        
        public void StopTime()
        {
            Context.GetTimeController().Stop();
        }

        public void ContinueTime()
        {
            Context.GetTimeController().Continue();
        }

        public void KillAllEnemy()
        {
            var ids = Context.GetLevelRuntimeInfo().GetAllEnemiesInfo().Keys.ToList();
            foreach (var id in ids)
            {
                Context.GetLevelController().DestroyEnemyCard(id);
            }
        }

        public int GetRemainEnemyCount()
        {
            return controller.TotalLevelNum - controller.CurProgress;
        }

        public void ClearAllEnemy()
        {
            controller.ClearAllEnemy();
        }

        public void AddEnemy(EnemyCard card)
        {
            controller.AddEnemy(card);
        }
    }
}