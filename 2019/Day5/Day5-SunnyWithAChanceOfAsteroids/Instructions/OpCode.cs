using System;
using System.Collections.Generic;
using System.Text;

namespace Day5_SunnyWithAChanceOfAsteroids.Instructions
{
    public enum OpCode
    {
        Add = 1,
        Multiply = 2,
        Input = 3,
        Output = 4,
        JumpIfTrue = 5,
        JumpIfFalse = 6,
        LessThan = 7,
        Equals = 8,
        Halt = 99
    }
}
