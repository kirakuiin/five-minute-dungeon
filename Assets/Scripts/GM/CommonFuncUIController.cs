using UnityEngine;

namespace GM
{
    /// <summary>
    /// 通用功能。 
    /// </summary>
    public class CommonFuncUIController : MonoBehaviour
    {
        private GmManager _manager;

        private void Start()
        {
            _manager = FindObjectOfType<GmManager>();
        }
        
        public void OnKillEnemy()
        {
            _manager.KillAllEnemy();
        }

        public void OnStopTime()
        {
            _manager.StopTime();
        }

        public void OnContinueTime()
        {
            _manager.ContinueTime();
        }
    }
}