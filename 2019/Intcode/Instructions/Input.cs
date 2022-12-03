using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Intcode.Instructions
{
    public class Input : IInstruction
    {
        private readonly IIntcodeState _state;

        public ParameterMode Param1Mode;

        public OpCode OpCode => OpCode.Input;

        public Input(IIntcodeState state, ParameterMode param1Mode = ParameterMode.Position)
        {
            _state = state;
            Param1Mode = param1Mode;
        }

        public int Execute()
        {
            int resultPosition = (int)GetParam1();
            _state[resultPosition] = _state.GetInput();

            return _state.PointerPosition + 2;
        }

        private BigInteger GetParam1() => ParameterHelper.GetAddress(Param1Mode, _state, _state.PointerPosition + 1);
    }
}
