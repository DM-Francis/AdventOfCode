using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day14
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] input = File.ReadAllLines("input.txt");

            Example();

            var program = new InitializationProgram(input);
            program.RunV2();

            long maxValid36bit = (long)Math.Pow(2, 36) - 1;
            long max = program.Memory.Values.Max();
            long sum = program.Memory.Values.Sum();

            Console.WriteLine(sum);
        }

        private static void Example()
        {
            string[] input = new[]
            {
                "mask = 000000000000000000000000000000X1001X",
                "mem[42] = 100",
                "mask = 00000000000000000000000000000000X0XX",
                "mem[26] = 1"
            };

            var program = new InitializationProgram(input);

            program.RunV2();
            long sum = program.Memory.Values.Sum();

            Console.WriteLine(sum);
        }
    }
}
