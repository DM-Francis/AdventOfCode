using Intcode.Instructions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Intcode
{
    public class IntcodeInterpreter : IIntcodeState
    {
        private readonly List<BigInteger> _memory;
        private readonly Action<BigInteger> _outputDelegate;
        private Func<BigInteger> _inputProvider;
        private readonly InstructionFactory _instructionFactory;

        public IReadOnlyList<BigInteger> Memory { get => _memory.AsReadOnly(); }
        public int PointerPosition { get; set; }
        public int RelativeBase { get; set; }

        public BigInteger this[int address]
        {
            get { ExpandMemoryFor(address); return _memory[address]; }
            set { ExpandMemoryFor(address); _memory[address] = value; }
        }

        private void ExpandMemoryFor(int address)
        {
            while (address >= _memory.Count)
            {
                _memory.Add(0);
            }
        }

        public IntcodeInterpreter(IEnumerable<int> program, Func<int> inputProvider, Action<int> outputDelegate)
        {
            _memory = new List<BigInteger>(program.Select(i => (BigInteger)i));
            _outputDelegate = b => outputDelegate.Invoke((int)b);
            _inputProvider = () => inputProvider.Invoke();
            _instructionFactory = new InstructionFactory(this);
        }

        public IntcodeInterpreter(IEnumerable<BigInteger> program) : this(program, () => 0, i => { }) { }
        public IntcodeInterpreter(IEnumerable<BigInteger> program, Action<BigInteger> outputDelegate) : this(program, () => 0, outputDelegate) { }
        public IntcodeInterpreter(IEnumerable<BigInteger> program, Func<BigInteger> inputProvider, Action<BigInteger> outputDelegate)
        {
            _memory = new List<BigInteger>(program);
            _outputDelegate = outputDelegate;
            _inputProvider = inputProvider;
            _instructionFactory = new InstructionFactory(this);
        }

        public void Interpret()
        {
            while (true)
            {
                IInstruction instruction = _instructionFactory.Get((int)_memory[PointerPosition]);

                if (instruction.OpCode == OpCode.Halt)
                {
                    break;
                }

                PointerPosition = instruction.Execute();
                if (PointerPosition >= _memory.Count)
                {
                    break;
                }
            }
        }

        public void Interpret(BigInteger input)
        {
            _inputProvider = () => input;
            Interpret();
        }

        public BigInteger GetInput() => _inputProvider.Invoke();
        public void Output(BigInteger value) => _outputDelegate.Invoke(value);
    }
}
