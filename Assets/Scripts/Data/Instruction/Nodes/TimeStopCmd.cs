using System.Threading.Tasks;

namespace Data.Instruction.Nodes
{
	/// <summary>
	/// 时间暂停。
	/// </summary>
	public class TimeStopCmd : CommandBase
	{
        public override async Task Execute(ICmdContext context, TempContext tempContext)
        {
            context.GetTimeController().Stop();
            await Task.CompletedTask;
        }
	}
}