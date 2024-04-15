using UnityEngine;

namespace GM
{
    public class GmUIController : MonoBehaviour
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