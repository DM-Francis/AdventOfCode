using System;
using System.Collections.Generic;
using System.Text;

namespace Intcode.Instructions
{
    public class Multiply : IInstruction
    {
        public ParameterMode Param1Mode { get; }
        public ParameterMode Param2Mode { get; }

        public OpCode OpCode => OpCode.Multiply;

        public Multiply(ParameterMode param1Mode, ParameterMode param2Mode)
        {
            this.Param1Mode = param1Mode;
            this.Param2Mode = param2Mode;
        }

        public int Execute(List<int> memory, int pointerPosition, int input, Action<int> outputDelegate)
        {
            int resultPosition = memory[pointerPosition + 3];
            memory[resultPosition] = GetParam1(memory, pointerPosition) * GetParam2(memory, pointerPosition);

            return pointerPosition + 4;
        }

        private int GetParam1(List<int> memory, int pointerPosition)
        {
            return Param1Mode.GetValue(memory, pointerPosition + 1);
        }

        private int GetParam2(List<int> memory, int pointerPosition)
        {
            return Param2Mode.GetValue(memory, pointerPosition + 2);
        }
    }
}
