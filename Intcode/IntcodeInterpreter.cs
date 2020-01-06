using Intcode.Instructions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Intcode
{
    public class IntcodeInterpreter
    {
        private readonly List<int> _memory;
        private readonly Action<int> _outputDelegate;
        private Func<int> _inputProvider;

        public IReadOnlyList<int> Memory { get => _memory.AsReadOnly(); }

        public IntcodeInterpreter(IEnumerable<int> program) : this(program, () => 0, i => { }) { }
        public IntcodeInterpreter(IEnumerable<int> program, Action<int> outputDelegate) : this(program, () => 0, outputDelegate) { }

        public IntcodeInterpreter(IEnumerable<int> program, Func<int> inputProvider, Action<int> outputDelegate)
        {
            _memory = new List<int>(program);
            _outputDelegate = outputDelegate;
            _inputProvider = inputProvider;
        }

        public void Interpret()
        {
            int pointerPosition = 0;

            while (true)
            {
                IInstruction instruction = InstructionFactory.Get(_memory[pointerPosition]);

                if (instruction.OpCode == OpCode.Halt)
                {
                    break;
                }

                pointerPosition = instruction.Execute(_memory, pointerPosition, _inputProvider, _outputDelegate);
                if (pointerPosition >= _memory.Count)
                {
                    break;
                }
            }
        }

        public void Interpret(int input)
        {
            _inputProvider = () => input;
            Interpret();
        }
    }
}
