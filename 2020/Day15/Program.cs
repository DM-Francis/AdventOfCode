using System;
using System.Collections.Generic;
using System.Linq;

namespace Day15
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] input = new[] { 0, 13, 1, 16, 6, 17 };

            var numbersPlayed = new List<int>(input);
            var previousNumberCache = new Dictionary<int, int>(input.Select((value, index) => new KeyValuePair<int, int>(value, index)));

            while(numbersPlayed.Count < 30000000)
            {
                PlayNextNumber(numbersPlayed, previousNumberCache);
            }

            Console.WriteLine(numbersPlayed[^1]);
        }

        private static void PlayNextNumber(List<int> numbersPlayed, Dictionary<int, int> previousNumberCache)
        {
            int previousNumber = numbersPlayed[^1];

            if (!previousNumberCache.ContainsKey(previousNumber))
            {
                previousNumberCache[previousNumber] = numbersPlayed.Count - 1;
                numbersPlayed.Add(0);
            }
            else
            {
                int indexMostRecent = numbersPlayed.Count - 1;
                int indexPrevious = previousNumberCache[previousNumber];

                previousNumberCache[previousNumber] = numbersPlayed.Count - 1;
                numbersPlayed.Add(indexMostRecent - indexPrevious);
            }
        }
    }
}
