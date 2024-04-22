using Data;
using GameLib.Common;
using GameLib.Common.Extension;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GM
{
    /// <summary>
    /// 敌方牌组控制。
    /// </summary>
    public class EnemyCardControlUIController : MonoBehaviour
    {
        [SerializeField] private Transform content;

        [SerializeField] private GameObject btnPrefab;

        [SerializeField] private Toggle firstBtn;

        [SerializeField] private TMP_Text remainText;
        
        private GmManager _manager;
        
        private void Awake()
        {
            _manager = FindObjectOfType<GmManager>();
        }

        private void OnEnable()
        {
            firstBtn.Select();
            SetRemainText();
        }

        private void SetRemainText()
        {
            if (_manager is null) return;
            remainText.text = $"{_manager.GetRemainEnemyCount()}";
        }

        public void OnSelectEvent(bool isSelect)
        {
            if (!isSelect) return;
            CleanScroll();
            foreach (var data in DataService.Instance.GetAllChallengeCard())
            {
                if (data.enemyCardType != EnemyCardType.Event) continue;
                CreateBtn($"{data.card}", new EnemyCard { type = data.enemyCardType, value = (int)data.card });
            }
        }
        
        private void CreateBtn(string desc, EnemyCard card)
        {
            var btn = GameObjectPool.Instance.Get(btnPrefab);
            btn.GetComponent<EnemyCardAddBtn>().Init(desc, () => OnClickEnemyBtn(card));
            btn.transform.SetParent(content);
        }

        private void OnClickEnemyBtn(EnemyCard card)
        {
            _manager.AddEnemy(card);
            SetRemainText();
        }

        private void CleanScroll()
        {
            content.DoSomethingToAllChildren(
                obj => GameObjectPool.Instance.ReturnWithReParent(obj, btnPrefab));
        }


        public void OnSelectMiniBoss(bool isSelect)
        {
            if (!isSelect) return;
            CleanScroll();
            foreach (var data in DataService.Instance.GetAllChallengeCard())
            {
                if (data.enemyCardType != EnemyCardType.MiniBoss) continue;
                CreateBtn($"{data.card}", new EnemyCard { type = data.enemyCardType, value = (int)data.card });
            }
        }
        
        public void OnSelectMonster(bool isSelect)
        {
            if (!isSelect) return;
            CleanScroll();
            foreach (var data in DataService.Instance.GetAllDoorData())
            {
                if (data.enemyCardType != EnemyCardType.Monster) continue;
                CreateBtn($"{data.card}", new EnemyCard { type = data.enemyCardType, value = (int)data.card });
            }
        }
        
        public void OnSelectPerson(bool isSelect)
        {
            if (!isSelect) return;
            CleanScroll();
            foreach (var data in DataService.Instance.GetAllDoorData())
            {
                if (data.enemyCardType != EnemyCardType.Person) continue;
                CreateBtn($"{data.card}", new EnemyCard { type = data.enemyCardType, value = (int)data.card });
            }
        }
        
        public void OnSelectObstacle(bool isSelect)
        {
            if (!isSelect) return;
            CleanScroll();
            foreach (var data in DataService.Instance.GetAllDoorData())
            {
                if (data.enemyCardType != EnemyCardType.Obstacle) continue;
                CreateBtn($"{data.card}", new EnemyCard { type = data.enemyCardType, value = (int)data.card });
            }
        }

        public void CleanAllEnemy()
        {
            _manager.ClearAllEnemy();
            SetRemainText();
        }
    }
}