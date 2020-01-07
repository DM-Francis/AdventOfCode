using System.Collections.Generic;
using System.Numerics;

namespace Intcode
{
    public interface IIntcodeState
    {
        BigInteger this[int address] { get; set; }

        int PointerPosition { get; set; }
        int RelativeBase { get; set; }

        BigInteger GetInput();
        void Output(BigInteger value);
    }
}