namespace Gameplay.Core
{
    /// <summary>
    /// 游戏规则。
    /// </summary>
    public static class GameRule
    {
        /// <summary>
        /// 获取玩家初始牌组数量。
        /// </summary>
        /// <param name="playerNum"></param>
        /// <returns></returns>
        public static int GetInitDeckNum(int playerNum)
        {
            return playerNum > 2 ? 40 : 80;
        }

        /// <summary>
        /// 获得玩家的初始手牌数量。
        /// </summary>
        /// <param name="playerNum"></param>
        /// <returns></returns>
        public static int GetInitHandNum(int playerNum)
        {
            switch (playerNum)
            {
                case 1:
                case 2:
                    return 5;
                case 3:
                    return 4;
                default:
                    return 3;
            }
        }

        /// <summary>
        /// 游戏时间。
        /// </summary>
        public static int CountdownTime => 300;
    }
}