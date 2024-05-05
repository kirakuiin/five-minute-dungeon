using System;
using Data;
using Gameplay.Core.State;
using Unity.Netcode;

namespace Gameplay.Core
{
    /// <summary>
    /// 游戏服务状态
    /// </summary>
    public class GamePlayServiceStatus : NetworkBehaviour
    {
        private PlayableChecker _checker;
        
        /// <summary>
        /// 状态改变事件。
        /// </summary>
        public event Action<GameServiceStatus> OnStateChanged;

        public event Action<ulong, SkillState> OnSkillStateChanged;

        /// <summary>
        /// 初始化。
        /// </summary>
        public void Init()
        {
            _checker = new PlayableChecker();
        }

        /// <summary>
        /// 当前状态。
        /// </summary>
        public GameServiceStatus CurrentStatus { private set; get; }
        
        /// <summary>
        /// 发布消息。(内部使用)
        /// </summary>
        /// <param name="status"></param>
        public void NotifyStatus(GameServiceStatus status)
        {
            NotifyStatusClientRpc(status);
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void NotifyStatusClientRpc(GameServiceStatus status)
        {
            CurrentStatus = status;
            OnStateChanged?.Invoke(status);
        }
        
        /// <summary>
        /// 更新技能状态。
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="status"></param>
        public void UpdateSkillState(ulong clientID, SkillState status)
        {
            UpdateSkillStateRpc(clientID, status);
        }
        
        [Rpc(SendTo.ClientsAndHost)]
        private void UpdateSkillStateRpc(ulong clientID, SkillState status)
        {
            OnSkillStateChanged?.Invoke(clientID, status);
        }
        
        /// <summary>
        /// 是否可以释放法术？
        /// </summary>
        public bool CanICastSkill(Skill skill)
        {
            return IsActionPhase() && _checker.CheckSkill(skill);
        }

        private bool IsActionPhase()
        {
            var isRightState = CurrentStatus.state is ServiceState.EventResolve or ServiceState.ListenAction;
            return isRightState && !CurrentStatus.IsEventResolving;
        }

        /// <summary>
        /// 是否可以打出卡牌？
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public bool CanIPlayThisCard(Card card)
        {
            return IsActionPhase() && _checker.CheckCard(card);
        }
    }
}