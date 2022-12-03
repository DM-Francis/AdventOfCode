using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day1_RocketEquation
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            PartTwo();
        }

        private static void PartOne()
        {
            List<int> moduleMasses = GetModuleMassesFromFile();

            int totalFuel = 0;

            foreach (int mass in moduleMasses)
            {
                totalFuel += GetFuelRequired(mass);
            }

            Console.WriteLine(totalFuel);
        }

        private static void PartTwo()
        {
            List<int> moduleMasses = GetModuleMassesFromFile();

            int totalFuel = 0;

            foreach(int moduleMass in moduleMasses)
            {
                int remainingMass = moduleMass;
                while (remainingMass > 0)
                {
                    int fuel = GetFuelRequired(remainingMass);
                    totalFuel += fuel;
                    remainingMass = fuel;
                }
            }

            Console.WriteLine(totalFuel);
        }

        private static List<int> GetModuleMassesFromFile()
        {
            string[] modulesMassesRaw = File.ReadAllLines("input.txt");
            List<int> moduleMasses = modulesMassesRaw.Select(s => int.Parse(s)).ToList();
            return moduleMasses;
        }

        private static int GetFuelRequired(int mass)
        {
            int fuel = mass / 3 - 2;

            if (fuel <= 0)
                return 0;
            else
                return fuel;
        }
    }
}
