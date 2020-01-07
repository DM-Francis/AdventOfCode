using System;
using System.Collections.Generic;
using System.Text;

namespace Intcode.Instructions
{
    public interface IInstruction
    {
        OpCode OpCode { get; }

        int Execute();
    }
}
