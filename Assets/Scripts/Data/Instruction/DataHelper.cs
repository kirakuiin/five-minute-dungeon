namespace Data.Instruction
{
    /// <summary>
    /// 数据帮助类。
    /// </summary>
    public static class DataHelper
    {
        /// <summary>
        /// 卡牌内是否含有指定类型的资源。
        /// </summary>
        /// <param name="card"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public static bool IsCardContainResource(Card card, Resource res)
        {
            var data = DataService.Instance.GetPlayerCardData(card);
            return data.playerCardType == PlayerCardType.ResourceCard
                   && data.action.GetAllResourceInfo().ContainsKey(res);
        }
    }
}