using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day08
{
    class HandheldConsole
    {
        private readonly List<Instruction> _instructions = new();
        public ReadOnlyCollection<Instruction> Instructions { get; }

        public int Accumulator { get; private set; }

        public HandheldConsole(IEnumerable<Instruction> instructions)
        {
            Instructions = _instructions.AsReadOnly();
            _instructions.AddRange(instructions);
        }

        public bool HasInfiniteLoop()
        {
            int position = 0;
            int[] instructionRunCounts = new int[_instructions.Count];

            while (position < _instructions.Count)
            {
                instructionRunCounts[position] += 1;
                if (instructionRunCounts[position] >= 2)
                    return true;

                position = RunInstruction(_instructions[position], position);
            }

            return false;
        }

        public void RunUntilInstructionExecutedTwice()
        {
            int position = 0;
            int[] instructionRunCounts = new int[_instructions.Count];

            while (instructionRunCounts.Max() < 2)
            {
                instructionRunCounts[position] += 1;
                if (instructionRunCounts[position] >= 2)
                    break;

                position = RunInstruction(_instructions[position], position);
            }
        }

        private int RunInstruction(Instruction instruction, int position)
        {
            if (instruction.Operation == Operation.nop)
            {
                return position + 1;
            }
            else if (instruction.Operation == Operation.acc)
            {
                Accumulator += instruction.Argument;
                return position + 1;
            }
            else if (instruction.Operation == Operation.jmp)
            {
                return position + instruction.Argument;
            }

            throw new ArgumentException($"Unrecognised operation {instruction.Operation}");
        }
    }
}
