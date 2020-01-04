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
            => Execute(memory, pointerPosition, () => input, outputDelegate);

        public int Execute(List<int> memory, int pointerPosition, Func<int> inputProvider, Action<int> outputDelegate)
        {
            int outputValue = ParameterHelper.GetValue(Param1Mode, memory, pointerPosition + 1);
            outputDelegate.Invoke(outputValue);

            return pointerPosition + 2;
        }
    }
}
