using System;
using System.Collections.Generic;
using System.Numerics;

namespace Day12_NBodyProblem
{
    class Program
    {
        static void Main(string[] args)
        {
            var moons = new List<Moon>
            {
                new Moon(new Vector3(-8, -9, -7)),
                new Moon(new Vector3(-5, 2, -1)),
                new Moon(new Vector3(11, 8, -14)),
                new Moon(new Vector3(1, -4, -11))
            };

            var testMoons = new List<Moon>
            {
                new Moon(new Vector3(-1,0,2)),
                new Moon(new Vector3(2,-10,-7)),
                new Moon(new Vector3(4,-8,8)),
                new Moon(new Vector3(3,5,-1))
            };

            // Part 1
            var system = new OrbitalSystem(moons);
            int steps = 1000;
            system.AdvanceTimeSteps(steps);
            Console.WriteLine($"Total energy after {steps} steps: {system.GetTotalEnergy()}");

            // Part 2
            var system2 = new OrbitalSystem(moons);
            long stepsUntilRepetition = system2.AdvanceUntilRepetition();

            Console.WriteLine($"Steps until repetition: {stepsUntilRepetition}");
        }
    }
}
