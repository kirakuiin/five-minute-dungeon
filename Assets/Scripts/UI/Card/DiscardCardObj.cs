using Data;
using DG.Tweening;
using GameLib.Common;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Card
{
    /// <summary>
    /// 被丢弃卡牌对象。
    /// </summary>
    public class DiscardCardObj : MonoBehaviour
    {
        [SerializeField] private Image cardImage;
        
        [Range(0.1f, 1f)] [SerializeField] private float playTime;

        private Vector3 _originScale;

        private GameObject _prefab;
        
        private void Awake()
        {
            _originScale = transform.localScale;
        }

        /// <summary>
        /// 初始化。
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="card"></param>
        public void Init(GameObject prefab, Data.Card card)
        {
            _prefab = prefab;
            cardImage.sprite = DataService.Instance.GetPlayerCardData(card).cardFront;
            transform.localScale = _originScale;
        }

        /// <summary>
        /// 移动向目标。
        /// </summary>
        /// <param name="target"></param>
        public void MoveToTarget(Transform target)
        {
            transform.DOScale(_originScale / 3, playTime);
            transform.DOMove(target.position, playTime)
                .OnComplete(() => GameObjectPool.Instance.ReturnWithReParent(gameObject, _prefab));
        }
    }
}