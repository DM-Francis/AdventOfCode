using System;
using System.Collections.Generic;
using System.Text;

namespace Day5_SunnyWithAChanceOfAsteroids.Instructions
{
    public interface IInstruction
    {
        OpCode OpCode { get; }

        int Execute(List<int> memory, int pointerPosition, int input, Action<int> outputDelegate);
    }
}
