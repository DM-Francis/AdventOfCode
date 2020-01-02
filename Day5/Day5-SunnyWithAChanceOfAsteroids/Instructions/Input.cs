using System;
using System.Collections.Generic;
using System.Text;

namespace Day5_SunnyWithAChanceOfAsteroids.Instructions
{
    public class Input : IInstruction
    {
        public OpCode OpCode => OpCode.Input;

        public int Execute(List<int> memory, int pointerPosition, int input, Action<int> outputDelegate)
        {
            int resultPosition = memory[pointerPosition + 1];
            memory[resultPosition] = input;

            return pointerPosition + 2;
        }
    }
}
