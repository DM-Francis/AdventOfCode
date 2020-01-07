using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using Intcode;

namespace Intcode.Instructions
{
    public class InstructionFactory
    {
        private readonly IIntcodeState _state;

        public InstructionFactory(IIntcodeState state)
        {
            _state = state;
        }

        public IInstruction Get(int instructionCode)
        {
            OpCode opCode = GetOpCodeFromInstruction(instructionCode);
            (ParameterMode param1Mode, ParameterMode param2Mode, ParameterMode param3Mode) = GetParameterModesFromInstruction(instructionCode);

            return opCode switch
            {
                OpCode.Add => new Add(_state, param1Mode, param2Mode, param3Mode),
                OpCode.Multiply => new Multiply(_state, param1Mode, param2Mode, param3Mode),
                OpCode.Input => new Input(_state, param1Mode),
                OpCode.Output => new Output(_state, param1Mode),
                OpCode.JumpIfTrue => new JumpIfTrue(_state, param1Mode, param2Mode),
                OpCode.JumpIfFalse => new JumpIfFalse(_state, param1Mode, param2Mode),
                OpCode.LessThan => new LessThan(_state, param1Mode, param2Mode, param3Mode),
                OpCode.Equals => new Equals(_state, param1Mode, param2Mode, param3Mode),
                OpCode.RelativeBaseOffet => new RelativeBaseOffset(_state, param1Mode),
                OpCode.Halt => new Halt(),
                _ => throw new InvalidOperationException($"Unrecognised OpCode {opCode.ToString()}")
            };
        }

        private static OpCode GetOpCodeFromInstruction(int instructionCode)
        {
            string instructionString = instructionCode.ToString();

            if (instructionString.Length <= 2)
            {
                return (OpCode)instructionCode;
            }
            else
            {
                string opCodeString = instructionString.Substring(instructionString.Length - 2, 2);
                return (OpCode)int.Parse(opCodeString);
            }
        }

        private static (ParameterMode param1Mode, ParameterMode param2Mode, ParameterMode param3Mode) GetParameterModesFromInstruction(BigInteger instructionCode)
        {
            var instructionCharsReversed = new List<char>(instructionCode.ToString().Reverse());

            if (instructionCharsReversed.Count <= 2)
            {
                return (ParameterMode.Position, ParameterMode.Position, ParameterMode.Position);
            }
            else
            {
                instructionCharsReversed.Add('0');
                instructionCharsReversed.Add('0');

                var instructionInts = instructionCharsReversed.Select(c => int.Parse(c.ToString())).ToList();

                return ((ParameterMode)instructionInts[2], (ParameterMode)instructionInts[3], (ParameterMode)instructionInts[4]);
            }
        }
    }
}
