using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Intcode.Instructions
{
    public class Output : IInstruction
    {
        private readonly IIntcodeState _state;

        public ParameterMode Param1Mode { get; }
        public OpCode OpCode => OpCode.Output;

        public Output(IIntcodeState state, ParameterMode param1Mode)
        {
            _state = state;
            Param1Mode = param1Mode;
        }

        public int Execute()
        {
            BigInteger outputValue = ParameterHelper.GetValue(Param1Mode, _state, _state.PointerPosition + 1);
            _state.Output(outputValue);

            return _state.PointerPosition + 2;
        }
    }
}
