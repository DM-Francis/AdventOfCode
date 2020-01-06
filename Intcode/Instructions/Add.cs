﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Intcode.Instructions
{
    public class Add : IInstruction
    {
        public ParameterMode Param1Mode { get; }
        public ParameterMode Param2Mode { get; }

        public OpCode OpCode => OpCode.Add;

        public Add(ParameterMode param1Mode, ParameterMode param2Mode)
        {
            Param1Mode = param1Mode;
            Param2Mode = param2Mode;
        }

        public int Execute(List<int> memory, int pointerPosition, int input, Action<int> outputDelegate)
            => Execute(memory, pointerPosition, () => input, outputDelegate);

        public int Execute(List<int> memory, int pointerPosition, Func<int> inputProvider, Action<int> outputDelegate)
        {
            int resultPosition = memory[pointerPosition + 3];
            memory[resultPosition] = GetParam1(memory, pointerPosition) + GetParam2(memory, pointerPosition);

            return pointerPosition + 4;
        }

        private int GetParam1(List<int> memory, int pointerPosition) => ParameterHelper.GetValue(Param1Mode, memory, pointerPosition + 1);
        private int GetParam2(List<int> memory, int pointerPosition) => ParameterHelper.GetValue(Param2Mode, memory, pointerPosition + 2);
    }
}
