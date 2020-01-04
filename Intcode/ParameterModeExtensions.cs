using System;
using System.Collections.Generic;
using System.Text;

namespace Intcode
{
    public static class ParameterModeExtensions
    {
        public static int GetValue(this ParameterMode mode, List<int> memory, int address)
        {
            return mode switch
            {
                ParameterMode.Position => memory[memory[address]],
                ParameterMode.Immediate => memory[address],
                _ => throw new InvalidOperationException($"Parameter Mode not recognised: {mode.ToString()}")
            };
        }
    }
}
