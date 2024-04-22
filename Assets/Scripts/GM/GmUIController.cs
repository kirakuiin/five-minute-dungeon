using UnityEngine;

namespace GM
{
    public class GmUIController : MonoBehaviour
    {
        [SerializeField] private GameObject common;

        [SerializeField] private GameObject enemy;
        
        public void OnSelectCommon(bool isSelected)
        {
            if (isSelected)
            {
                common.SetActive(true);
                enemy.SetActive(false);
            }
        }

        public void OnSelectEnemy(bool isSelected)
        {
            if (isSelected)
            {
                common.SetActive(false);
                enemy.SetActive(true);
            }
        }
    }
}