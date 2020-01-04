using System;
using System.Collections.Generic;
using System.Text;

namespace Intcode.Instructions
{
    public class Output : IInstruction
    {
        public ParameterMode Param1Mode { get; }
        public OpCode OpCode => OpCode.Output;

        public Output(ParameterMode param1Mode)
        {
            this.Param1Mode = param1Mode;
        }

        public int Execute(List<int> memory, int pointerPosition, int input, Action<int> outputDelegate)
        {
            int outputValue = Param1Mode.GetValue(memory, pointerPosition + 1);
            outputDelegate.Invoke(outputValue);

            return pointerPosition + 2;
        }
    }
}
