using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Day12_NBodyProblem
{
    public class OrbitalSystem
    {
        private long _timesteps = 0;
        private OrbitalSystemState _state;

        private OrbitalSystemSingleDimension _x;
        private OrbitalSystemSingleDimension _y;
        private OrbitalSystemSingleDimension _z;

        public MoonState Moons => new(new[]
        {
            new Moon(new Vector3(_x.A.Position, _y.A.Position, _z.A.Position), new Vector3(_x.A.Velocity, _y.A.Velocity, _z.A.Velocity)),
            new Moon(new Vector3(_x.B.Position, _y.B.Position, _z.B.Position), new Vector3(_x.B.Velocity, _y.B.Velocity, _z.B.Velocity)),
            new Moon(new Vector3(_x.C.Position, _y.C.Position, _z.C.Position), new Vector3(_x.C.Velocity, _y.C.Velocity, _z.C.Velocity)),
            new Moon(new Vector3(_x.D.Position, _y.D.Position, _z.D.Position), new Vector3(_x.D.Velocity, _y.D.Velocity, _z.D.Velocity)),
        });

        public OrbitalSystem(List<Moon> moons)
        {
            _state = new OrbitalSystemState(moons);
            _x = new OrbitalSystemSingleDimension
            {
                A = new MoonSingleDimension(_state.A.Position.X),
                B = new MoonSingleDimension(_state.B.Position.X),
                C = new MoonSingleDimension(_state.C.Position.X),
                D = new MoonSingleDimension(_state.D.Position.X)
            };
            _y = new OrbitalSystemSingleDimension
            {
                A = new MoonSingleDimension(_state.A.Position.Y),
                B = new MoonSingleDimension(_state.B.Position.Y),
                C = new MoonSingleDimension(_state.C.Position.Y),
                D = new MoonSingleDimension(_state.D.Position.Y)
            };
            _z = new OrbitalSystemSingleDimension
            {
                A = new MoonSingleDimension(_state.A.Position.Z),
                B = new MoonSingleDimension(_state.B.Position.Z),
                C = new MoonSingleDimension(_state.C.Position.Z),
                D = new MoonSingleDimension(_state.D.Position.Z)
            };
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
            _x.ApplyTimestep();
            _y.ApplyTimestep();
            _z.ApplyTimestep();
            _timesteps++;
        }

        public long AdvanceUntilRepetition()
        {
            long xTimesteps = AdvanceUntilRepetitionInSingleDimension(_x);
            long yTimesteps = AdvanceUntilRepetitionInSingleDimension(_y);
            long zTimesteps = AdvanceUntilRepetitionInSingleDimension(_z);

            return Euclid.LeastCommonMultiple(xTimesteps, yTimesteps, zTimesteps);
        }

        private static long AdvanceUntilRepetitionInSingleDimension(OrbitalSystemSingleDimension dimension)
        {
            var initialState = dimension;
            var state = dimension;
            long timesteps = 0;
            do
            {
                state.ApplyTimestep();
                timesteps++;
                if (timesteps % 100_000_000 == 0)
                    Console.WriteLine($"Timestep: {timesteps:N0}");
            }
            while (!state.Equals(initialState));

            return timesteps;
        }

        public float GetTotalEnergy() => _state.TotalEnergy;
    }
}
