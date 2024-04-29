using Data;
using Gameplay.Camera;
using TMPro;
using UI.Common;
using UnityEngine;

namespace UI.Model
{
    /// <summary>
    /// 敌方模型UI控制器。
    /// </summary>
    public class EnemyInitUIController : InitComponent
    {
        [SerializeField] private Transform uiRoot;
        
        [SerializeField] private TMP_Text textName;
        
        public override void Init()
        {
            InitUI();
        }

        private void InitUI()
        {
            var enemyCard = GetComponent<EnemyModel>().Card;
            var data = DataService.Instance.GetEnemyCardData(enemyCard);
            textName.text = enemyCard.type switch
            {
                EnemyCardType.Boss => "",
                _ => data.desc,
            };
        }

        private void LateUpdate()
        {
            uiRoot.transform.rotation = CameraControl.Instance.ActiveCamera.transform.rotation;
        }
    }
}