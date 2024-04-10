using System.Collections.Generic;
using System.Linq;

namespace Data.Instruction
{
    /// <summary>
    /// 指令运行时所需要的上下文环境。
    /// </summary>
    public interface ICmdContext
    {
        /// <summary>
        /// 获得玩家控制器对象。
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        public IPlayerController GetPlayerController(ulong clientID);

        /// <summary>
        /// 获得全部的玩家ID
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ulong> GetAllClientIDs();

        /// <summary>
        /// 获得关卡控制器。
        /// </summary>
        /// <returns></returns>
        public ILevelController GetLevelController();

        /// <summary>
        /// 获得玩家控制器对象列表。
        /// </summary>
        /// <param name="playerList"></param>
        /// <returns></returns>
        public IEnumerable<IPlayerController> GetPlayerControllers(IEnumerable<ulong> playerList)
        {
            return playerList.Select(GetPlayerController);
        }
    }

    /// <summary>
    /// 关卡控制器。
    /// </summary>
    public interface ILevelController
    {
        /// <summary>
        /// 向当前关卡内添加资源。
        /// </summary>
        /// <param name="type"></param>
        /// <param name="num"></param>
        public void AddResource(Resource type, int num = 1);

        /// <summary>
        /// 直接消灭敌人卡牌。
        /// </summary>
        /// <param name="enemyID"></param>
        public void DestroyEnemyCard(ulong enemyID);

        /// <summary>
        /// 揭示接下来的n个关卡。
        /// </summary>
        /// <param name="num"></param>
        public void RevealNextLevel(int num);

        /// <summary>
        /// 停止游戏计时。
        /// </summary>
        public void StopTime();

        /// <summary>
        /// 获得当前在场敌人卡ID。
        /// </summary>
        public IEnumerable<ulong> GetEnemyIDs();
    }
}