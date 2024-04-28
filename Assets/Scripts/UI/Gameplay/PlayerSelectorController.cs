using System.Collections.Generic;
using Data.Check;
using GameLib.Common.Extension;
using Gameplay.Camera;
using Gameplay.Core;
using UI.Model;
using UnityEngine;

namespace UI.Gameplay
{
    /// <summary>
    /// 玩家选择器。
    /// </summary>
    public class PlayerSelectorController : MonoBehaviour
    {
        [SerializeField] private PlayerModelController modelController;
            
        private IPlayerSelector Selector { set; get; }

        private readonly List<ulong> _selectList = new();

        private int _needNum;
        
        /// <summary>
        /// 初始化。
        /// </summary>
        public void Init()
        {
            Selector = GamePlayContext.Instance.GetPlayerRuntimeInfo().GetRuntimeInteractive().GetPlayerSelector();
            Selector.OnPlayerSelecting += OnPlayerSelecting;
        }

        private void OnPlayerSelecting(int num, bool canSelectSelf)
        {
            _selectList.Clear();
            _needNum = num;
            modelController.PlayersModel.Apply(
                model => model.GetComponent<PlayerModelSelector>().EnterSelectMode(canSelectSelf, SetSelectPlayer)
            );
            CameraControl.Instance.ActivePlayerCamera();
        }

        private void SetSelectPlayer(ulong playerID, bool isSelect)
        {
            if (isSelect)
            {
                _selectList.Add(playerID);
            }
            else
            {
                _selectList.Remove(playerID);
            }
            CheckCondition();
        }

        private void CheckCondition()
        {
            if (_selectList.Count < _needNum) return;
            Selector.SelectPlayer(_selectList);
            CameraControl.Instance.ActiveMain();
            modelController.PlayersModel.Apply(
                model => model.GetComponent<PlayerModelSelector>().ExitSelectMode()
            );
        }
    }
}