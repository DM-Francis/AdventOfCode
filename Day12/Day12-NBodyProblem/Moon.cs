using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Day12_NBodyProblem
{
    public class Moon
    {
        public Vector3 Position { get; set; }
        public Vector3 Velocity { get; set; } = Vector3.Zero;

        public float PotentialEnergy => SumAbsValues(Position);
        public float KineticEnergy => SumAbsValues(Velocity);
        public float TotalEnergy => PotentialEnergy * KineticEnergy;

        public Moon(Vector3 position)
        {
            Position = position;
        }

        public Moon(Vector3 position, Vector3 velocity)
        {
            Position = position;
            Velocity = velocity;
        }

        private float SumAbsValues(Vector3 vector)
        {
            var absVector = Vector3.Abs(vector);
            return absVector.X + absVector.Y + absVector.Z;
        }
    }
}
