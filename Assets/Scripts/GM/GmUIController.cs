using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GM
{
    public class GmUIController : MonoBehaviour
    {
        [SerializeField] private List<ToggleConfig> toggleConfigs;

        private void Start()
        {
            foreach (var config in toggleConfigs)
            {
                config.toggle.onValueChanged.AddListener((isSelect) => OnSelected(isSelect, config.subDialog));
            }
        }

        private void OnDestroy()
        {
            foreach (var config in toggleConfigs)
            {
                config.toggle.onValueChanged.RemoveAllListeners();
            }
        }

        private void OnSelected(bool isSelected, GameObject dialog)
        {
            if (!isSelected) return;
            foreach (var config in toggleConfigs.Where(config => config.subDialog != dialog))
            {
                config.subDialog.SetActive(false);
            }
            dialog.SetActive(true);
        }
    }
    
    [Serializable]
    public struct ToggleConfig
    {
        public Toggle toggle;
        public GameObject subDialog;
    }
}