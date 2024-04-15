using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Test
{
    public class Test : MonoBehaviour
    {
        [SerializeField] private TMP_Text progressText;
    
        [SerializeField] private TMP_Text enemyInfo;

        [SerializeField] private Button btn;

        public void OnClick()
        {
            progressText.text = "abc";
        }

        public void OnUI(InputAction.CallbackContext callback)
        {
            Debug.Log($"{callback}");
            enemyInfo.text = ("点击了");
        }
    }
}
