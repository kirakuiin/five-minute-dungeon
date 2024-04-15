using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Data;

namespace UI.Card
{
    /// <summary>
    /// 设置卡牌外观。
    /// </summary>
    [RequireComponent(typeof(CardRuntimeData))]
    public class CardAppearanceSetter : MonoBehaviour
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

        private CardRuntimeData _data;

        /// <summary>
        /// 初始化卡牌。
        /// </summary>
        public void Init()
        {
            _data = GetComponent<CardRuntimeData>();
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
    }
}
