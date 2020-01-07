using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Intcode.Tests
{
    public static class IntcodeTestHelper
    {
        public static StubIntcodeState CreateState(this List<BigInteger> memory)
        {
            return new StubIntcodeState { Memory = memory };
        }
    }
}
