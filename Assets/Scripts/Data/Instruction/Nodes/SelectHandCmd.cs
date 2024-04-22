using System.Collections.Generic;
using System.Linq;
using XNode;
using System.Threading.Tasks;

namespace Data.Instruction.Nodes
{
    /// <summary>
    /// 选择手牌指令。
    /// </summary>
    public class SelectHandCmd : CommandBase
    {
        /// <summary>
        /// 选择的手牌。
        /// </summary>
        [Output] public List<Card> cardList;

        /// <summary>
        /// 需要进行选择的玩家(仅第一个生效)。
        /// </summary>
        [Input] public List<ulong> playerList;

        /// <summary>
        /// 选择的数量。
        /// </summary>
        public int num;

        public override object GetValue(NodePort port)
        {
            return cardList;
        }
        
        public override async Task Execute(ICmdContext context, TempContext tempContext)
        {
            playerList = GetInputValue<List<ulong>>(nameof(playerList));
            var player = context.GetPlayerController(playerList.First());
            if (player.HandCards.Count <= num)
            {
                cardList = new List<Card>(player.HandCards);
            }
            else
            {
                var handler = player.GetInteractiveHandler();
                cardList = await handler.SelectHandCards(num);
            }
        }
    }
}