using System;
using System.Collections.Generic;
using System.Text;

namespace Intcode.Instructions
{
    public class RelativeBaseOffset : IInstruction
    {
        private readonly IIntcodeState _state;

        public OpCode OpCode => OpCode.RelativeBaseOffet;
        public ParameterMode Param1Mode { get; }

        public RelativeBaseOffset(IIntcodeState state, ParameterMode param1Mode)
        {
            _state = state;
            Param1Mode = param1Mode;
        }

        public int Execute()
        {
            _state.RelativeBase += (int)ParameterHelper.GetValue(Param1Mode, _state, _state.PointerPosition + 1);

            return _state.PointerPosition + 2;
        }
    }
}
