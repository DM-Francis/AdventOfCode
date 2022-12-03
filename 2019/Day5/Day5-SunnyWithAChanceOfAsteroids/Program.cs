using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day5_SunnyWithAChanceOfAsteroids
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var program = GetProgramFromFile();

            var interpreter = new IntcodeInterpreter(program, v => Console.WriteLine(v));
            interpreter.Interpret(5);
        }

        private static List<int> GetProgramFromFile()
        {
            string programRaw = File.ReadAllText("input.txt");

            return programRaw.Split(',').Select(s => int.Parse(s)).ToList();
        }
    }
}
