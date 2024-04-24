using System.Collections.Generic;
using Data.Check;
using Gameplay.Core;
using TMPro;
using UI.Card;
using Unity.Mathematics;
using UnityEngine;

namespace UI.Gameplay
{
    /// <summary>
    /// 手牌区选择卡牌控制器。
    /// </summary>
    public class HandZoneSelectorController : MonoBehaviour
    {
        [SerializeField] private GameObject mask;
        
        [SerializeField] private TMP_Text discardText;
        
        private IHandSelector _selector;

        private int _needNum;

        private readonly List<Data.Card> _selectedList = new();

        private HandZoneUIController _handUI;
        

        /// <summary>
        /// 初始化。
        /// </summary>
        public void Init(HandZoneUIController uiController)
        {
            _handUI = uiController;
            _selector = GamePlayContext.Instance.GetPlayerRuntimeInfo().GetRuntimeInteractive().GetHandSelector();
            _selector.OnHandSelecting += OnHandSelecting;
        }

        private void OnHandSelecting(int num)
        {
            _selectedList.Clear();
            _needNum = math.min(num, GamePlayContext.Instance.GetPlayerRuntimeInfo().GetHands().Count);
            if (IsSatisfy())
            {
                _selector.SelectHand(_selectedList);
            }
            else
            {
                EnterSelectMode(_needNum);
            }
        }

        private void EnterSelectMode(int num)
        {
            mask.SetActive(true);
            discardText.text = $"丢弃{num}张手牌";
            _handUI.EnterSelectMode();
        }
        
        private bool IsSatisfy()
        {
            return _selectedList.Count == _needNum;
        }


        private void ExitSelectMode()
        {
            mask.SetActive(false);
            _handUI.ExitSelectMode();
        }

        /// <summary>
        /// 选择卡牌。
        /// </summary>
        public void SelectCard(Data.Card card)
        {
            _selectedList.Add(card);
            if (IsSatisfy())
            {
                ExitSelectMode();
                _selector.SelectHand(_selectedList);
            }
        }

        /// <summary>
        /// 取消选中。
        /// </summary>
        /// <param name="card"></param>
        public void UnSelect(Data.Card card)
        {
            _selectedList.Remove(card);
        }
    }
}