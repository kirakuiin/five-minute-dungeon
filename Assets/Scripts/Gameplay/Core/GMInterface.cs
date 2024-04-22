using Data;

namespace Gameplay.Core
{
    /// <summary>
    /// GM控制接口
    /// </summary>
    public interface IGmLevelControl
    {
        /// <summary>
        /// 清理所有关卡。
        /// </summary>
        public void ClearAllLevel();

        /// <summary>
        /// 添加一个敌人。
        /// </summary>
        /// <param name="card"></param>
        public void AddEnemy(EnemyCard card);
    }
}