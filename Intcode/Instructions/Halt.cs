using System;
using System.Collections.Generic;
using System.Text;

namespace Intcode.Instructions
{
    public class Halt : IInstruction
    {
        public OpCode OpCode => OpCode.Halt;

        public int Execute()
            => throw new NotSupportedException();
    }
}
