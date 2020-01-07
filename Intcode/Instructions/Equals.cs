using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Intcode.Instructions
{
    public class Equals : IInstruction
    {
        private readonly IIntcodeState _state;

        public ParameterMode Param1Mode { get; }
        public ParameterMode Param2Mode { get; }
        public ParameterMode Param3Mode { get; }

        public OpCode OpCode => OpCode.Equals;

        public Equals(IIntcodeState state, ParameterMode param1Mode, ParameterMode param2Mode, ParameterMode param3Mode = ParameterMode.Position)
        {
            _state = state;
            Param1Mode = param1Mode;
            Param2Mode = param2Mode;
            Param3Mode = param3Mode;
        }

        public int Execute()
        {
            int resultPosition = (int)GetParam3();

            if (GetParam1() == GetParam2())
            {
                _state[resultPosition] = 1;
            }
            else
            {
                _state[resultPosition] = 0;
            }

            return _state.PointerPosition + 4;
        }

        private BigInteger GetParam1() => ParameterHelper.GetValue(Param1Mode, _state, _state.PointerPosition + 1);
        private BigInteger GetParam2() => ParameterHelper.GetValue(Param2Mode, _state, _state.PointerPosition + 2);
        private BigInteger GetParam3() => ParameterHelper.GetAddress(Param3Mode, _state, _state.PointerPosition + 3);
    }
}
