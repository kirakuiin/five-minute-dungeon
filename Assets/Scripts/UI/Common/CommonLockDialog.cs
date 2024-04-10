using Popup;
using TMPro;
using UnityEngine;

namespace UI.Common
{
    public class CommonLockDialog : MonoBehaviour, ILockDialog
    {
        [SerializeField]
        private TMP_Text info;
        
        public void SetString(string text)
        {
            info.text = text;
            gameObject.SetActive(true);
        }
    }
}