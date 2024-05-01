using System.Linq;
using Data;
using GameLib.Common;
using Gameplay.Core;
using Gameplay.Data;

namespace Gameplay.Progress
{
    /// <summary>
    /// 管理游戏进度信息。
    /// </summary>
    public class GameProgress : PersistentMonoSingleton<GameProgress>
    {
        /// <summary>
        /// 当前挑战boss
        /// </summary>
        public Boss CurrentBoss { private set; get; } = Boss.BabyBarbarian;

        /// <summary>
        /// 是否存在下一个Boss。
        /// </summary>
        /// <returns></returns>
        public bool HasNextBoss()
        {
            return CurrentBoss != Boss.FinalForm;
        }

        /// <summary>
        /// boss的挑战进度。
        /// </summary>
        public float BossPercent => (float)((int)CurrentBoss + 1) / ((int)Boss.FinalForm + 1);

        /// <summary>
        /// 挑战下一个Boss
        /// </summary>
        public void ChallengeNextBoss()
        {
            CurrentBoss = GetNextBoss();
        }
        
        private Boss GetNextBoss()
        {
            return CurrentBoss + 1;
        }

        /// <summary>
        /// 获得挑战结果数据。
        /// </summary>
        public ChallengeResultData ChallengeResult { private set; get; }

        /// <summary>
        /// 生成挑战结果。
        /// </summary>
        public void GenerateChallengeResult()
        {
            var context = GamePlayContext.Instance;
            var data = new ChallengeResultData();
            var levelInfo = context.GetLevelController();
            data.isWin = levelInfo.IsComplete();
            data.boss = CurrentBoss;
            var timeInfo = context.GetTimeRuntimeInfo();
            data.useTime = GameRule.CountdownTime-timeInfo.RemainTime;
            if (!data.isWin)
            {
                data.reason = timeInfo.RemainTime <= 0 ? FailureReason.Timeout : FailureReason.CardExhausted;
            }
            data.squadList = context.GetAllClientIDs().Select(id => context.GetPlayerRuntimeInfo(id).PlayerClass).ToList();

            ChallengeResult = data;
        }

        /// <summary>
        /// 更新挑战结果。
        /// </summary>
        /// <param name="data"></param>
        public void UpdateResult(ChallengeResultData data)
        {
            ChallengeResult = data;
            CurrentBoss = data.boss;
        }

        public void Reset()
        {
            CurrentBoss = Boss.BabyBarbarian;
        }
    }
}