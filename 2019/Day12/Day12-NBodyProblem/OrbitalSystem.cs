using System;
using System.Collections.Generic;

namespace Day12_NBodyProblem
{
    public class OrbitalSystem
    {
        private long _timesteps = 0;
        private OrbitalSystemState _state;

        public MoonState Moons => new MoonState(new[] {_state.A, _state.B, _state.C, _state.D});


        public OrbitalSystem(List<Moon> moons)
        {
            _state = new OrbitalSystemState(moons);
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
            _state.ApplyTimestep();
            _timesteps++;
        }

        public long AdvanceUntilRepetition()
        {
            var initialState = _state;

            var state = _state;
            do
            {
                state.ApplyTimestep();
                _timesteps++;
                if (_timesteps % 100_000_000 == 0)
                    Console.WriteLine($"Timestep: {_timesteps:N0}  Total energy: {state.TotalEnergy}");
            }
            while (!state.Equals(initialState));

            return _timesteps;
        }

        public float GetTotalEnergy() => _state.TotalEnergy;
    }
}
