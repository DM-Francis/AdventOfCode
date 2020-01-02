using System;
using System.Collections.Generic;
using System.Text;

namespace Day5_SunnyWithAChanceOfAsteroids.Instructions
{
    public class JumpIfFalse : IInstruction
    {
        public ParameterMode Param1Mode { get; }
        public ParameterMode Param2Mode { get; }

        public OpCode OpCode => OpCode.JumpIfFalse;

        public JumpIfFalse(ParameterMode param1Mode, ParameterMode param2Mode)
        {
            Param1Mode = param1Mode;
            Param2Mode = param2Mode;
        }

        public int Execute(List<int> memory, int pointerPosition, int input, Action<int> outputDelegate)
        {
            if (GetParam1(memory, pointerPosition) == 0)
            {
                return GetParam2(memory, pointerPosition);
            }
            else
            {
                return pointerPosition + 3;
            }
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
