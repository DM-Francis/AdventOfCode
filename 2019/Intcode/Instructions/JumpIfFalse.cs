using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Intcode.Instructions
{
    public class JumpIfFalse : IInstruction
    {
        private readonly IIntcodeState _state;

        public ParameterMode Param1Mode { get; }
        public ParameterMode Param2Mode { get; }

        public OpCode OpCode => OpCode.JumpIfFalse;

        public JumpIfFalse(IIntcodeState state, ParameterMode param1Mode, ParameterMode param2Mode)
        {
            _state = state;
            Param1Mode = param1Mode;
            Param2Mode = param2Mode;
        }

        public int Execute()
        {
            if (GetParam1() == 0)
            {
                return (int)GetParam2();
            }
            else
            {
                return _state.PointerPosition + 3;
            }
        }

        private BigInteger GetParam1() => ParameterHelper.GetValue(Param1Mode, _state, _state.PointerPosition + 1);
        private BigInteger GetParam2() => ParameterHelper.GetValue(Param2Mode, _state, _state.PointerPosition + 2);
    }
}
