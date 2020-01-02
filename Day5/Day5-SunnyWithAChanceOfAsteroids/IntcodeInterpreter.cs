using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Day5_SunnyWithAChanceOfAsteroids
{
    public class IntcodeInterpreter
    {
        private readonly List<int> _program;
        private readonly Action<int> _outputDelegate;

        public IReadOnlyList<int> Memory { get => _program.AsReadOnly(); }

        public IntcodeInterpreter(IEnumerable<int> program) : this(program, i => { })
        {
        }

        public IntcodeInterpreter(IEnumerable<int> program, Action<int> outputDelegate)
        {
            _program = new List<int>(program);
            _outputDelegate = outputDelegate;
        }

        public void Interpret()
        {
            Interpret(0);
        }

        public void Interpret(int input)
        {
            int pointerPosition = 0;

            while (true)
            {
                int opCode = _program[pointerPosition];

                if (opCode == 99)
                {
                    break;
                }
                
                int addressesToJump = RunOpCodeInstruction(opCode, pointerPosition, input);
                pointerPosition += addressesToJump;

                if (pointerPosition >= _program.Count)
                {
                    break;
                }
            }
        }

        private int RunOpCodeInstruction(int opCode, int pointerPosition, int input)
        {
            switch (opCode)
            {
                case 1:
                    Add(pointerPosition);
                    return 4;
                case 2:
                    Multiply(pointerPosition);
                    return 4;
                case 3:
                    Input(pointerPosition, input);
                    return 2;
                case 4:
                    Output(pointerPosition);
                    return 2;
                default:
                    throw new InvalidOperationException($"Unknown Opcode {opCode}");
            }
        }

        private void Output(int pointerPosition)
        {
            int operandPosition = _program[pointerPosition + 1];
            _outputDelegate.Invoke(_program[operandPosition]);
        }

        private void Input(int pointerPosition, int input)
        {
            int savePosition = _program[pointerPosition + 1];
            _program[savePosition] = input;
        }

        private void Add(int pointerPosition)
        {
            int operand1position = _program[pointerPosition + 1];
            int operand2position = _program[pointerPosition + 2];
            int resultPosition = _program[pointerPosition + 3];

            _program[resultPosition] = _program[operand1position] + _program[operand2position];
        }

        private void Multiply(int pointerPosition)
        {
            int operand1position = _program[pointerPosition + 1];
            int operand2position = _program[pointerPosition + 2];
            int resultPosition = _program[pointerPosition + 3];

            _program[resultPosition] = _program[operand1position] * _program[operand2position];
        }
    }
}
