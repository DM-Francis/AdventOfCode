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
        public Dictionary<MoonState, long> History = new Dictionary<MoonState, long>();
        public MoonState Moons { get; }

        private long _timesteps = 0;
        private readonly List<(Moon A, Moon B)> _moonPairs;

        public OrbitalSystem(List<Moon> moons)
        {
            Moons = new MoonState(moons);
            var combs = new Combinations<Moon>(Moons.ToList(), 2);
            _moonPairs = combs.Select(l => (l[0], l[1])).ToList();
        }

        public void AdvanceTimeSteps(long timesteps)
        {
            for (long i = 1; i <= timesteps; i++)
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

        public void AdvanceUntilRepetition()
        {
            RecordCurrentState();
            AdvanceTimeSteps(1_000_000);
            
            if (History.TryGetValue(Moons, out long matchingTimeStep))
            {

            }
        }

        public float GetTotalEnergy() => Moons.Select(m => m.TotalEnergy).Sum();

        private void RecordCurrentState()
        {
            History[Moons.GetCopy()] = _timesteps;
        }

        private void ApplyGravity()
        {
            for (int i = 0; i < _moonPairs.Count; i++)
            {
                ApplyGravityToPair(_moonPairs[i].A, _moonPairs[i].B);
            }
        }

        private void ApplyGravityToPair(Moon moonA, Moon moonB)
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
            for (int i = 0; i < Moons.Count; i++)
            {
                Moons[i].Position += Moons[i].Velocity;
            }
        }
    }
}
