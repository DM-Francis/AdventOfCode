using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Intcode.Tests
{
    public class StubIntcodeState : IIntcodeState
    {
        public List<BigInteger> Memory { get; set; }

        public BigInteger InputValue { get; set; }
        public BigInteger OutputValue { get; set; }

        public BigInteger this[int address]
        {
            get => Memory[address];
            set => Memory[address] = value;
        }

        public int PointerPosition { get; set; }
        public int RelativeBase { get; set; }

        public BigInteger GetInput() => InputValue;
        public void Output(BigInteger value) => OutputValue = value;
    }
}
