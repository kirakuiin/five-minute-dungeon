using System;

namespace Data.Instruction
{
    /// <summary>
    /// 指令执行错误。
    /// </summary>
    public class InstructionException : Exception
    {
        public InstructionException(string reason) : base()
        {
        }
    }
}