using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day10
{
    static class Program
    {
        static void Main(string[] args)
        {
            List<int> numbers = File.ReadAllLines("input.txt").Select(l => int.Parse(l)).ToList();

            Example();
            GetTotalCombinationsForNumbers(numbers);
        }

        public static long GetTotalCombinationsForNumbers(IEnumerable<int> numbersInput)
        {
            var numbers = new List<int>(numbersInput);
            numbers.Add(0);
            numbers.Add(numbers.Max() + 3);
            numbers.Sort();

            int[] differences = new int[numbers.Count];
            for (int i = 1; i < numbers.Count; i++)
            {
                differences[i] = numbers[i] - numbers[i - 1];
            }

            int diff1Count = differences.Count(d => d == 1);
            int diff3Count = differences.Count(d => d == 3);

            Console.WriteLine(diff1Count * diff3Count);

            // Get groups of 1 differences
            // First identify indexes of all 3 differences
            var diff3Indexes = new List<int>();
            diff3Indexes.Add(0);
            for (int i = 0; i < differences.Length; i++)
            {
                if (differences[i] == 3)
                    diff3Indexes.Add(i);
            }

            int[] diff2ndTier = new int[diff3Indexes.Count];
            for (int i = 1; i < diff3Indexes.Count; i++)
            {
                diff2ndTier[i] = diff3Indexes[i] - diff3Indexes[i - 1];
            }

            diff2ndTier = diff2ndTier.Select(d => d - 1).ToArray();

            var combinationMap = new Dictionary<int, int>
            {
                { -1 ,1 },
                { 0, 1 },
                { 1, 1 },
                { 2, 2 },
                { 3, 4 },
                { 4, 7 },
                { 5, 13 }
            };

            int max = diff2ndTier.Max();

            var combinations = diff2ndTier.Select(d => (long)combinationMap[d]);

            long totalCombinations = combinations.Aggregate((a, b) => a * b);

            return totalCombinations;
        }

        private static void Example()
        {
            var numbers1 = new List<int>
            {
                16,
                10,
                15,
                5,
                1,
                11,
                7,
                19,
                6,
                12,
                4,
            };

            var numbers2 = new List<int>
            {
                28,
                33,
                18,
                42,
                31,
                14,
                46,
                20,
                48,
                47,
                24,
                23,
                49,
                45,
                19,
                38,
                39,
                11,
                1,
                32,
                25,
                35,
                8,
                17,
                7,
                9,
                4,
                2,
                34,
                10,
                3,
            };

            GetTotalCombinationsForNumbers(numbers1);
            GetTotalCombinationsForNumbers(numbers2);
        }
    }
}
