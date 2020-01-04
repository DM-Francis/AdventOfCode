using System;
using System.Collections.Generic;
using System.IO;

namespace Day6_UniversalOrbitMap
{
    class Program
    {
        static void Main(string[] args)
        {
            var universe = LoadUniverseFromFile();
            Console.WriteLine(universe.CountOrbits());
            Console.WriteLine(universe.GetDistanceBetween("YOU", "SAN"));
        }

        private static Universe LoadUniverseFromFile()
        {
            var orbitsRaw = new List<string>(File.ReadAllLines("input.txt"));
            var universe = new Universe();

            foreach(var orbit in orbitsRaw)
            {
                universe.AddRawSpaceObject(orbit);
            }

            return universe;
        }
    }
}
