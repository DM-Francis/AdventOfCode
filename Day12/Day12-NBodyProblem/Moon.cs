using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text;

namespace Day12_NBodyProblem
{
    public class Moon : IEquatable<Moon>
    {
        public Vector3 Position { get; set; }
        public Vector3 Velocity { get; set; }

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

        public Moon GetCopy()
        {
            return new Moon(Position, Velocity);
        }

        private float SumAbsValues(Vector3 vector)
        {
            var absVector = Vector3.Abs(vector);
            return absVector.X + absVector.Y + absVector.Z;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Moon);
        }

        public bool Equals([AllowNull] Moon other)
        {
            return other != null &&
                   Position.Equals(other.Position) &&
                   Velocity.Equals(other.Velocity);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Position, Velocity);
        }
    }
}
