using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day2_1202ProgramAlarm
{
    public class Program
    {
        public static void Main(string[] args)
        {
            PartTwo();
        }

        private static void PartTwo()
        {
            List<int> program = GetProgramFromFile();

            var zeroTo99 = Enumerable.Range(0, 100);
            var pairsToTry = (from noun in zeroTo99
                              from verb in zeroTo99
                              select new { noun, verb }).ToList();

            foreach(var pair in pairsToTry)
            {
                var updatedProgram = UpdateProgram(program, pair.noun, pair.verb);
                var interpreter = new IntcodeInterpreter(updatedProgram);
                interpreter.Interpret();

                if (interpreter.State[0] == 19690720)
                {
                    Console.WriteLine(100 * pair.noun + pair.verb);
                    break;
                }
            }

            
        }

        private static List<int> UpdateProgram(List<int> program, int noun, int verb)
        {
            var newProgram = new List<int>(program);
            newProgram[1] = noun;
            newProgram[2] = verb;

            return newProgram;
        }

        private static void PartOne()
        {
            List<int> program = GetProgramFromFile();
            PrepareProgram(program);

            var interpreter = new IntcodeInterpreter(program);
            interpreter.Interpret();

            Console.WriteLine(interpreter.State[0]);
        }

        private static void PrepareProgram(List<int> program)
        {
            program[1] = 12;
            program[2] = 2;
        }

        private static List<int> GetProgramFromFile()
        {
            string programRaw = File.ReadAllText("input.txt");

            return programRaw.Split(',').Select(s => int.Parse(s)).ToList();
        }
    }
}
