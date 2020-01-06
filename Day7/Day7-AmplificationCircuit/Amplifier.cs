using Day7_AmplificationCircuit.Signals;
using Intcode;
using System;
using System.Collections.Generic;

namespace Day7_AmplificationCircuit
{
    public class Amplifier
    {
        private readonly IntcodeInterpreter _interpreter;
        private int _inputCount = 0;

        private readonly IInputSignal _input;
        private readonly IOutputSignal _output;

        public int PhaseSetting { get; }

        public Amplifier(IEnumerable<int> program, int phaseSetting, IInputSignal input, IOutputSignal output)
        {
            _interpreter = new IntcodeInterpreter(program, ProgramInput, ProgramOutput);
            PhaseSetting = phaseSetting;
            _input = input;
            _output = output;
        }

        public void Run()
        {
            _interpreter.Interpret();
        }

        private int ProgramInput()
        {
            return _inputCount++ == 0 ? PhaseSetting : _input.Get();
        }

        private void ProgramOutput(int output)
        {
            _output.Set(output);
        }
    }
}
