using System;
using System.Collections.Generic;
using System.Text;

namespace Day5_SunnyWithAChanceOfAsteroids.Instructions
{
    public class Equals : IInstruction
    {
        public ParameterMode Param1Mode { get; }
        public ParameterMode Param2Mode { get; }

        public OpCode OpCode => OpCode.Equals;

        public Equals(ParameterMode param1Mode, ParameterMode param2Mode)
        {
            Param1Mode = param1Mode;
            Param2Mode = param2Mode;
        }

        public int Execute(List<int> memory, int pointerPosition, int input, Action<int> outputDelegate)
        {
            int resultPosition = memory[pointerPosition + 3];
            if (GetParam1(memory, pointerPosition) == GetParam2(memory, pointerPosition))
            {
                memory[resultPosition] = 1;
            }
            else
            {
                memory[resultPosition] = 0;
            }

            return pointerPosition + 4;
        }

        private int GetParam1(List<int> memory, int pointerPosition) => Param1Mode.GetValue(memory, pointerPosition + 1);
        private int GetParam2(List<int> memory, int pointerPosition) => Param2Mode.GetValue(memory, pointerPosition + 2);
    }
}
