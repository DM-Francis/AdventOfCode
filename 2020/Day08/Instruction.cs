using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day08
{
    public record Instruction
    {
        public Operation Operation { get; init; }
        public int Argument { get; init; }

        public Instruction() { }

        public Instruction(string rawIntruction)
        {
            string[] instructionSplit = rawIntruction.Split(' ');
            Operation = Enum.Parse<Operation>(instructionSplit[0]);
            Argument = int.Parse(instructionSplit[1]);
        }
    }
}
