using System;
using System.Collections.Generic;
using System.Linq;

namespace Day4_SecureContainer
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            // Get all possible ints that it could be
            // For each int:
            // Check for at least one set of equal adjacent digits
            // Check the digits are ascending

            int count = 0;
            int start = 138307;
            int end = 654504;
            foreach (int i in GetIntegersInRange(start, end))
            {
                if (HasSomeEqualAdjacentDigits(i) && DigitsAreAscending(i) && HasAPairOf2EqualDigits(i))
                {
                    count++;
                }
            }

            Console.WriteLine(count);
        }

        public static List<int> GetIntegersInRange(int start, int end)
        {
            return Enumerable.Range(start, end - start + 1).ToList();
        }

        public static bool HasSomeEqualAdjacentDigits(int i)
        {
            var digits = GetDigits(i);

            int previous = -1;
            foreach (var digit in digits)
            {
                if (digit == previous) return true;
                previous = digit;
            }

            return false;
        }

        public static bool HasAPairOf2EqualDigits(int i)
        {
            var digits = GetDigits(i);

            var countPerDigit = from digit in digits
                                group digit by digit into digitGroup
                                select digitGroup.Count();

            return countPerDigit.Contains(2);
        }

        public static bool DigitsAreAscending(int i)
        {
            var digits = GetDigits(i);

            int previous = -1;
            foreach (var digit in digits)
            {
                if (digit < previous) return false;
                previous = digit;
            }

            return true;
        }

        private static IEnumerable<int> GetDigits(int i)
        {
            string intAsString = i.ToString();
            var digits = intAsString.Select(c => int.Parse(c.ToString()));
            return digits;
        }
    }
}
