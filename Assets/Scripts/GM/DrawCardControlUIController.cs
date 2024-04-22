using Data;
using GameLib.Common;
using GameLib.Common.Extension;
using UnityEngine;
using UnityEngine.UI;

namespace GM
{
    /// <summary>
    /// 敌方牌组控制。
    /// </summary>
    public class DrawCardControlUIController: MonoBehaviour
    {
        [SerializeField] private Transform content;

        [SerializeField] private Transform playerScroll;

        [SerializeField] private GameObject btnPrefab;

        [SerializeField] private GameObject togglePrefab;

        [SerializeField] private Toggle firstBtn;

        [SerializeField] private ToggleGroup playerGroup;

        private GmManager _manager;

        private ulong _currentSelectedID;
        
        private void Awake()
        {
            _manager = FindObjectOfType<GmManager>();
        }

        private void OnEnable()
        {
            firstBtn.Select();
        }

        private void Start()
        {
            GeneratePlayerList();
            playerScroll.GetChild(0).GetComponent<Toggle>().Select();
        }

        private void GeneratePlayerList()
        {
            foreach (var id in _manager.GetAllClientIDs())
            {
                var obj = Instantiate(togglePrefab, playerScroll);
                obj.GetComponent<Toggle>().group = playerGroup;
                obj.GetComponent<PlayerSelectBtn>().Init($"玩家{id}", () => OnChangeClientID(id));
            }
        }

        private void OnChangeClientID(ulong clientID)
        {
            _currentSelectedID = clientID;
        }

        public void OnSelectResource(bool isSelect)
        {
            if (!isSelect) return;
            CleanScroll();
            foreach (var data in DataService.Instance.GetAllPlayerCard())
            {
                if (data.playerCardType != PlayerCardType.ResourceCard) continue;
                CreateBtn($"{data.card}", data.card);
            }
        }
        
        private void CreateBtn(string desc, Card card)
        {
            var btn = GameObjectPool.Instance.Get(btnPrefab);
            btn.GetComponent<CardAddBtn>().Init(desc, () => OnClickCardBtn(card));
            btn.transform.SetParent(content);
        }

        private void OnClickCardBtn(Card card)
        {
            _manager.AddDrawPileCard(_currentSelectedID, card);
        }

        private void CleanScroll()
        {
            content.DoSomethingToAllChildren(
                obj => GameObjectPool.Instance.ReturnWithReParent(obj, btnPrefab));
        }


        public void OnSelectAction(bool isSelect)
        {
            if (!isSelect) return;
            CleanScroll();
            foreach (var data in DataService.Instance.GetAllPlayerCard())
            {
                if (data.playerCardType != PlayerCardType.ActionCard) continue;
                CreateBtn($"{data.card}", data.card);
            }
        }

        public void CleanAllDrawPile()
        {
            _manager.ClearAllDrawPile(_currentSelectedID);
        }
    }
}