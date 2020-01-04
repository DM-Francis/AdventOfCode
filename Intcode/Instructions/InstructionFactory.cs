using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Intcode.Instructions
{
    public static class InstructionFactory
    {
        public static IInstruction Get(int instructionCode)
        {
            OpCode opCode = GetOpCodeFromInstruction(instructionCode);
            (ParameterMode param1Mode, ParameterMode param2Mode, ParameterMode param3Mode) = GetParameterModesFromInstruction(instructionCode);

            return opCode switch
            {
                OpCode.Add => new Add(param1Mode, param2Mode),
                OpCode.Multiply => new Multiply(param1Mode, param2Mode),
                OpCode.Input => new Input(),
                OpCode.Output => new Output(param1Mode),
                OpCode.JumpIfTrue => new JumpIfTrue(param1Mode, param2Mode),
                OpCode.JumpIfFalse => new JumpIfFalse(param1Mode, param2Mode),
                OpCode.LessThan => new LessThan(param1Mode, param2Mode),
                OpCode.Equals => new Equals(param1Mode, param2Mode),
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

        private static (ParameterMode param1Mode, ParameterMode param2Mode, ParameterMode param3Mode) GetParameterModesFromInstruction(int instructionCode)
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
