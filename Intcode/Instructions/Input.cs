using System;
using System.Collections.Generic;
using System.Text;

namespace Intcode.Instructions
{
    public class Input : IInstruction
    {
        public OpCode OpCode => OpCode.Input;

        public int Execute(List<int> memory, int pointerPosition, int input, Action<int> outputDelegate)
            => Execute(memory, pointerPosition, () => input, outputDelegate);

        public int Execute(List<int> memory, int pointerPosition, Func<int> inputProvider, Action<int> outputDelegate)
        {
            int resultPosition = memory[pointerPosition + 1];
            memory[resultPosition] = inputProvider.Invoke();

            return pointerPosition + 2;
        }
    }
}
