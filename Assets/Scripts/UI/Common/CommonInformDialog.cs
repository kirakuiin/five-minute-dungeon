using System;
using GameLib.Common;
using UnityEngine;
using Popup;
using TMPro;


namespace UI.Common
{
    public class CommonInformDialog : MonoBehaviour, IInformDialog
    {
        [SerializeField]
        private TMP_Text informText;
        
        public void SetString(string text)
        {
            informText.text = text;
        }

        public void OnClick()
        {
            Destroy(gameObject);
        }
    }
}