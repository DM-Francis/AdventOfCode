using Day5_SunnyWithAChanceOfAsteroids.Instructions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Day5_SunnyWithAChanceOfAsteroids
{
    public class IntcodeInterpreter
    {
        private readonly List<int> _memory;
        private readonly Action<int> _outputDelegate;

        public IReadOnlyList<int> Memory { get => _memory.AsReadOnly(); }

        public IntcodeInterpreter(IEnumerable<int> program) : this(program, i => { })
        {
        }

        public IntcodeInterpreter(IEnumerable<int> program, Action<int> outputDelegate)
        {
            _memory = new List<int>(program);
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
                IInstruction instruction = InstructionFactory.Get(_memory[pointerPosition]);

                if (instruction.OpCode == OpCode.Halt)
                {
                    break;
                }

                pointerPosition = instruction.Execute(_memory, pointerPosition, input, _outputDelegate);
                if (pointerPosition >= _memory.Count)
                {
                    break;
                }
            }
        }
    }
}
