using System;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Lobby
{
    /// <summary>
    /// 选择职业事件。
    /// </summary>
    [Serializable]
    public class ClassSelectedEvent : UnityEvent<ClassData>
    {
    }
    
    public class ClassGridController : MonoBehaviour
    {
        [SerializeField] private TMP_Text className;

        [SerializeField] private TMP_Text skillName;

        [SerializeField] private TMP_Text skillDesc;

        [SerializeField] private Image backgroundImg;

        [SerializeField] private Transform gridTransform;

        [SerializeField] private GameObject avatarPrefab;

        [SerializeField] private ToggleGroup group;

        private ClassData _data;

        /// <summary>
        /// 选择职业确认时触发。
        /// </summary>
        public ClassSelectedEvent onClassSelected;

        private void Start()
        {
            InitGrid();
        }

        private void InitGrid()
        {
            foreach (var data in DataService.Instance.GetAllClassData())
            {
                var obj = Instantiate(avatarPrefab, gridTransform);
                obj.GetComponent<ClassAvatarItemController>().Init(data, group, OnSelectClass);
            }
        }

        private void OnSelectClass(ClassData data)
        {
            className.text = data.className;
            skillName.text = data.skillData.skillName;
            skillDesc.text = data.skillData.skillDesc;
            backgroundImg.sprite = data.deckData.deckBack;
            _data = data;
        }

        public void OnConfirm()
        {
            onClassSelected?.Invoke(_data);
            gameObject.SetActive(false);
        }
    }
}