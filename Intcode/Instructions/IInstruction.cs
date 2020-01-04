using System;
using System.Collections.Generic;
using System.Text;

namespace Intcode.Instructions
{
    public interface IInstruction
    {
        OpCode OpCode { get; }

        int Execute(List<int> memory, int pointerPosition, int input, Action<int> outputDelegate);

        int Execute(List<int> memory, int pointerPosition, Func<int> inputProvider, Action<int> outputDelegate);
    }
}
