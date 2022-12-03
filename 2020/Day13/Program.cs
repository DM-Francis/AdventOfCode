using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day13
{
    class Program
    {
        static void Main(string[] args)
        {
            PartTwo();
        }

        private static void PartOne()
        {
            string[] input = File.ReadAllLines("input.txt");

            int startTime = int.Parse(input[0]);
            int[] validBuses = input[1].Split(',').Where(s => s != "x").Select(s => int.Parse(s)).ToArray();

            int currentTime = startTime;
            bool match = validBuses.Any(b => currentTime % b == 0); ;
            while (!match)
            {
                currentTime++;
                match = validBuses.Any(b => currentTime % b == 0);
            }

            int validBus = validBuses.First(b => currentTime % b == 0);

            Console.WriteLine(validBus * (currentTime - startTime));
        }

        private static void PartTwo()
        {
            string[] input = File.ReadAllLines("input.txt");

            string[] busIDs = input[1].Split(',');

            var busConstraints = new List<(long Index, long BusId)>();

            for (int i = 0; i < busIDs.Length; i++)
            {
                if (busIDs[i] != "x")
                    busConstraints.Add((i, int.Parse(busIDs[i])));
            }

            busConstraints.Sort((a, b) => (int)b.BusId - (int)a.BusId); // Largest bus id first

            long timestampToCheck = 0;
            bool matchesAll = false;
            long increment = 1;

            while (!matchesAll)
            {
                Console.WriteLine(increment);
                timestampToCheck += increment;
                var matches = new List<long>();
                foreach(var (index, busID) in busConstraints)
                {
                    if ((timestampToCheck + index) % busID == 0)
                        matches.Add(busID);
                    else
                        break;
                }

                if (matches.Count == busConstraints.Count)
                    matchesAll = true;
                else if (matches.Count > 0)
                    increment = matches.Aggregate((a, b) => a * b);
            }

            Console.WriteLine(timestampToCheck);
        }
    }
}
