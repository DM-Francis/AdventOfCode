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

            var system = new OrbitalSystem(moons);

            system.AdvanceTimeSteps(46_000_000);

            Console.WriteLine(system.GetTotalEnergy());
        }
    }
}
