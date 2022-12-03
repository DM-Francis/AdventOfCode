using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day09
{
    class Program
    {
        static void Main(string[] args)
        {
            long[] numbers = File.ReadAllLines("input.txt").Select(s => long.Parse(s)).ToArray();

            long invalidNum = FindFirstInvalidNumber(numbers);
            Console.WriteLine(invalidNum);

            long[] set = FindSetThatSumsToNumber(invalidNum, numbers);

            Console.WriteLine(set.Min() + set.Max());
        }

        private static long FindFirstInvalidNumber(long[] numbers)
        {
            for (int i = 25; i < numbers.Length; i++)
            {
                long[] previous25 = numbers[(i - 25)..i];

                var validSums = new List<long>();
                for (int j = 0; j < previous25.Length; j++)
                {
                    for (int k = 0; k < previous25.Length; k++)
                    {
                        if (previous25[j] != previous25[k])
                            validSums.Add(previous25[j] + previous25[k]);
                    }
                }

                if (!validSums.Contains(numbers[i]))
                {
                    return numbers[i];
                }
            }

            throw new InvalidDataException("Invalid number not found");
        }

        private static long[] FindSetThatSumsToNumber(long target, long[] allNumbers)
        {
            for (int i = 0; i < allNumbers.Length - 1; i++)
            {
                long sum = allNumbers[i];
                for (int j = i + 1; j < allNumbers.Length; j++)
                {
                    sum += allNumbers[j];

                    if (sum == target)
                        return allNumbers[i..(j + 1)];
                    else if (sum > target)
                        break;
                }
            }

            throw new InvalidOperationException("Set not found");
        }
    }
}
