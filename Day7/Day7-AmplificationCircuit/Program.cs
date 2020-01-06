using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day7_AmplificationCircuit
{
    static class Program
    {
        static void Main(string[] args)
        {
            PartTwo();
        }

        private static void PartOne()
        {
            var program = GetProgramFromFile();
            var possibleSettings = new List<int> { 0, 1, 2, 3, 4 };

            var allPermutations = GetPermutations(possibleSettings, 5);

            var maxOutput = allPermutations.Select(p => GetOutputForPhaseSettings(program, p)).Max();

            Console.WriteLine(maxOutput);
        }

        private static void PartTwo()
        {
            var program = GetProgramFromFile();
            var possibleSettings = new List<int> { 5, 6, 7, 8, 9 };

            var allPermutations = GetPermutations(possibleSettings, 5);

            var maxOutput = allPermutations.Select(p => GetOutputForPhaseSettings(program, p, true)).Max();

            Console.WriteLine(maxOutput);
        }

        private static List<int> GetProgramFromFile()
        {
            string programRaw = File.ReadAllText("input.txt");

            return programRaw.Split(',').Select(c => int.Parse(c)).ToList();
        }

        private static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
        {
            if (length == 1)
            {
                return list.Select(t => new T[] { t });
            }

            return GetPermutations(list, length - 1)
                        .SelectMany(t => list.Where(o => !t.Contains(o)),
                            (t1, t2) => t1.Concat(new T[] { t2 }));
        }

        private static int GetOutputForPhaseSettings(IEnumerable<int> program, IEnumerable<int> phaseSettings, bool withFeedbackLoop = false)
        {
            var circuit = new AmplificationCircuit(program, phaseSettings, withFeedbackLoop);
            return circuit.GetOutputSignal();
        }
    }
}
