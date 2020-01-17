using Combinatorics.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Day12_NBodyProblem
{
    public class OrbitalSystem
    {
        private readonly List<Moon> _moons;
        private int _timesteps = 0;

        public OrbitalSystem(List<Moon> moons)
        {
            _moons = moons;
        }

        public void AdvanceManyTimeSteps(int timesteps)
        {
            for (int i = 1; i <= timesteps; i++)
            {
                AdvanceTimeStep();
            }
        }

        public void AdvanceTimeStep()
        {
            ApplyGravity();
            ApplyVelocity();
            _timesteps++;
        }

        public float GetTotalEnergy() => _moons.Select(m => m.TotalEnergy).Sum();

        private void ApplyGravity()
        {
            var moonPairs = new Combinations<Moon>(_moons, 2);

            foreach(var moonPair in moonPairs)
            {
                ApplyGravity(moonPair[0], moonPair[1]);
            }
        }

        private void ApplyGravity(Moon moonA, Moon moonB)
        {
            var diff = moonB.Position - moonA.Position;
            var unitChange = diff / Vector3.Abs(diff);

            if (float.IsNaN(unitChange.X)) unitChange.X = 0;
            if (float.IsNaN(unitChange.Y)) unitChange.Y = 0;
            if (float.IsNaN(unitChange.Z)) unitChange.Z = 0;

            moonA.Velocity += unitChange;
            moonB.Velocity -= unitChange;
        }

        private void ApplyVelocity()
        {
            foreach(var moon in _moons)
            {
                moon.Position += moon.Velocity;
            }
        }

    }
}
