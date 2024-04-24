using Data;
using GameLib.Animation;
using GameLib.Common;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Card
{
    /// <summary>
    /// 被丢弃卡牌对象。
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(MoveAction))]
    [RequireComponent(typeof(ScaleAction))]
    public class DiscardCardObj : MonoBehaviour
    {
        [SerializeField] private Image cardImage;
        
        [Range(0.1f, 1f)] [SerializeField] private float playTime;
        
        private MoveAction _move;

        private ScaleAction _scale;

        private Vector3 _originScale;

        private GameObject _prefab;
        
        private void Awake()
        {
            _move = GetComponent<MoveAction>();
            _scale = GetComponent<ScaleAction>();
            _move.time = playTime;
            _scale.time = playTime;
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
            _scale.ScaleTo(transform, _originScale/3);
            _move.MoveTo(transform, target.position, () => GameObjectPool.Instance.ReturnWithReParent(gameObject, _prefab));
        }

        private void OnValidate()
        {
            if (_scale)
            {
                _scale.time = playTime;
            }
            if (_move)
            {
                _move.time = playTime;
            }
        }
    }
}