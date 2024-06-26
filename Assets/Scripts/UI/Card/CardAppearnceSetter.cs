using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Data;
using UI.Common;

namespace UI.Card
{
    /// <summary>
    /// 设置卡牌外观。
    /// </summary>
    [RequireComponent(typeof(ICardData))]
    public class CardAppearanceSetter : InitComponent
    {
        [SerializeField]
        private Image cardFrontUI;

        [SerializeField]
        private TMP_Text cardNameUI;

        [SerializeField]
        private TMP_Text cardDescUI;

        [SerializeField]
        private TMP_Text cardNumUI;

        [SerializeField]
        private Transform textBaseTransform;

        [SerializeField]
        private GameObject selectImg;

        private ICardData _data;

        /// <summary>
        /// 初始化卡牌。
        /// </summary>
        public override void Init()
        {
            _data = GetComponent<ICardData>();
            InitUI();
        }

        /// <summary>
        /// 设置卡牌数量。
        /// </summary>
        /// <param name="num"></param>
        public void SetNum(int num)
        {
            cardNumUI.gameObject.SetActive(num > 0);
            cardNumUI.text = $"X{num}";
        }

        private void InitUI()
        {
            var cardData = DataService.Instance.GetPlayerCardData(_data.Card);
            cardFrontUI.sprite = cardData.cardFront;
            cardNameUI.text = cardData.playerCardType == PlayerCardType.ActionCard ? cardData.cardName : "";
            cardDescUI.text = cardData.cardDescription;
            SetNum(0);
            MoveTextBase(cardData);
        }

        private void MoveTextBase(CardData cardData)
        {
            textBaseTransform.Translate(new Vector3(0, cardData.textOffset));
        }

        public void SetSelected(bool isSelected)
        {
            selectImg.SetActive(isSelected);
        }
    }
}
