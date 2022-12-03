using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Intcode
{
    public static class ParameterHelper
    {
        public static BigInteger GetValue(ParameterMode mode, IIntcodeState state, int parameterAddress)
        {
            return mode switch
            {
                ParameterMode.Position => state[(int)state[parameterAddress]],
                ParameterMode.Immediate => state[parameterAddress],
                ParameterMode.Relative => state[(int)(state[parameterAddress] + state.RelativeBase)],
                _ => throw new InvalidOperationException($"Parameter Mode not recognised: {mode.ToString()}")
            };
        }

        public static int GetAddress(ParameterMode mode, IIntcodeState state, int parameterAddress)
        {
            return mode switch
            {
                ParameterMode.Position => (int)state[parameterAddress],
                ParameterMode.Immediate => parameterAddress,
                ParameterMode.Relative => (int)(state[parameterAddress] + state.RelativeBase),
                _ => throw new InvalidOperationException($"Parameter Mode not recognised: {mode.ToString()}")
            };
        }
    }
}
