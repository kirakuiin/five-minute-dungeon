﻿using Data;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Gameplay
{
    /// <summary>
    /// 资源图标。
    /// </summary>
    public class ResIconControllerUI : MonoBehaviour
    {
        [SerializeField] private Image image;

        [SerializeField] private Animator animator;

        [SerializeField] private GameObject mask;

        /// <summary>
        /// 初始化。
        /// </summary>
        /// <param name="type"></param>
        public void Init(Resource type)
        {
            mask.SetActive(false);
            InitUI(type);
        }

        private void InitUI(Resource type)
        {
            if (type == Resource.Wild)
            {
                animator.enabled = true;
                animator.SetTrigger("PlayChange");
            }
            else
            {
                animator.enabled = false;
                image.sprite = DataService.Instance.GetResourceData(type).icon;
                image.color = Color.white;
            }
        }
        
        /// <summary>
        /// 置灰.
        /// </summary>
        public void SetGrey()
        {
            mask.SetActive(true);
        }

    }
}