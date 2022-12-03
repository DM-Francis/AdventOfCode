using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day08
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] rawInput = File.ReadAllLines("input.txt");

            var instructions = rawInput.Select(i => new Instruction(i)).ToList();
            var nopOrsJmpIndexes = new List<int>();

            for (int i = 0; i < instructions.Count; i++)
            {
                if (instructions[i].Operation == Operation.nop || instructions[i].Operation == Operation.jmp)
                    nopOrsJmpIndexes.Add(i);
            }

            foreach(int index in nopOrsJmpIndexes)
            {
                var newInstructions = new List<Instruction>(instructions);

                if (newInstructions[index].Operation == Operation.nop)
                {
                    newInstructions[index] = new Instruction
                    {
                        Operation = Operation.jmp,
                        Argument = newInstructions[index].Argument
                    };

                    var console = new HandheldConsole(newInstructions);

                    if (!console.HasInfiniteLoop())
                    {
                        Console.WriteLine(console.Accumulator);
                        break;
                    }
                }
                else if (newInstructions[index].Operation == Operation.jmp)
                {
                    newInstructions[index] = new Instruction
                    {
                        Operation = Operation.nop,
                        Argument = newInstructions[index].Argument
                    };

                    var console = new HandheldConsole(newInstructions);

                    if (!console.HasInfiniteLoop())
                    {
                        Console.WriteLine(console.Accumulator);
                        break;
                    }
                }
            }
        }
    }
}
