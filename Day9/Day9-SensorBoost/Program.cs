using Intcode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace Day9_SensorBoost
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = GetProgramFromFile();

            var interpreter = new IntcodeInterpreter(program, b => Console.WriteLine(b));
            interpreter.Interpret(2);
        }

        private static List<BigInteger> GetProgramFromFile()
        {
            string[] programRaw = File.ReadAllText("input.txt").Split(',');

            return programRaw.Select(s => BigInteger.Parse(s)).ToList();
        }
    }
}
