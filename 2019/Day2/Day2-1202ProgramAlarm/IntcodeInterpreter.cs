using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Day2_1202ProgramAlarm
{
    public class IntcodeInterpreter
    {
        private readonly List<int> _program;

        public IReadOnlyList<int> State { get => _program.AsReadOnly(); }

        public IntcodeInterpreter(IEnumerable<int> program)
        {
            _program = new List<int>(program);
        }

        public void Interpret()
        {
            bool halt = false;
            int currentPosition = 0;

            while (!halt)
            {
                switch (_program[currentPosition])
                {
                    case 1:
                        Add(currentPosition);
                        break;
                    case 2:
                        Multiply(currentPosition);
                        break;
                    case 99:
                        halt = true;
                        break;
                    default:
                        throw new InvalidOperationException($"Unknown Opcode {_program[currentPosition]}");
                }

                currentPosition += 4;

                if (currentPosition >= _program.Count)
                {
                    halt = true;
                }
            }
        }

        private void Add(int opcodePosition)
        {
            int operand1position = _program[opcodePosition + 1];
            int operand2position = _program[opcodePosition + 2];
            int resultPosition = _program[opcodePosition + 3];

            _program[resultPosition] = _program[operand1position] + _program[operand2position];
        }

        private void Multiply(int opcodePosition)
        {
            int operand1position = _program[opcodePosition + 1];
            int operand2position = _program[opcodePosition + 2];
            int resultPosition = _program[opcodePosition + 3];

            _program[resultPosition] = _program[operand1position] * _program[operand2position];
        }
    }
}
