using System;
using System.Numerics;

namespace Day12_NBodyProblem
{
    public struct Moon : IEquatable<Moon>
    {
        public Vector3 Position;
        public Vector3 Velocity;

        public float PotentialEnergy => SumAbsValues(Position);
        public float KineticEnergy => SumAbsValues(Velocity);
        public float TotalEnergy => PotentialEnergy * KineticEnergy;

        public Moon(Vector3 position)
        {
            Position = position;
            Velocity = Vector3.Zero;
        }

        public Moon(Vector3 position, Vector3 velocity)
        {
            Position = position;
            Velocity = velocity;
        }

        public void AddVelocityAndApplyToPosition(Vector3 velocityChange)
        {
            var newVelocity = Velocity + velocityChange;
            Position += newVelocity;
            Velocity = newVelocity;
        }

        private static float SumAbsValues(Vector3 vector)
        {
            var absVector = Vector3.Abs(vector);
            return Vector3.Dot(absVector, Vector3.One);
        }

        public bool Equals(Moon other)
        {
            return Position.Equals(other.Position) && Velocity.Equals(other.Velocity);
        }
    }
}
