using Data;
using Data.Check;
using Unity.Netcode;

namespace Gameplay.Core
{
    /// <summary>
    /// 检查技能和卡牌是否合法。
    /// </summary>
    public class PlayableChecker
    {
        private readonly IRuntimeContext _context = GamePlayContext.Instance;

        private readonly ulong _clientID = NetworkManager.Singleton.LocalClientId;

        /// <summary>
        /// 检查出牌是否合法。
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public bool CheckCard(Card card)
        {
            var data = DataService.Instance.GetPlayerCardData(card);
            return data.check.Execution(_context, _clientID);
        }

        /// <summary>
        /// 检查是否可以释放技能。
        /// </summary>
        /// <param name="skill"></param>
        /// <returns></returns>
        public bool CheckSkill(Skill skill)
        {
            var data = DataService.Instance.GetSkillData(skill);
            return data.check.Execution(_context, _clientID);
        }
    }
}