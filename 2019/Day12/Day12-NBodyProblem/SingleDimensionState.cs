using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Day12_NBodyProblem
{
    public struct OrbitalSystemSingleDimension : IEquatable<OrbitalSystemSingleDimension>
    {
        public MoonSingleDimension A;
        public MoonSingleDimension B;
        public MoonSingleDimension C;
        public MoonSingleDimension D;

        public void ApplyTimestep()
        {
            var diffab = GetGravityDiff(A, B);
            var diffac = GetGravityDiff(A, C);
            var diffad = GetGravityDiff(A, D);
            var diffbc = GetGravityDiff(B, C);
            var diffbd = GetGravityDiff(B, D);
            var diffcd = GetGravityDiff(C, D);

            var deltaA = diffab + diffac + diffad;
            var deltaB = -diffab + diffbc + diffbd;
            var deltaC = -diffac - diffbc + diffcd;
            var deltaD = -diffad - diffbd - diffcd;

            A.AddVelocityAndApplyToPosition(deltaA);
            B.AddVelocityAndApplyToPosition(deltaB);
            C.AddVelocityAndApplyToPosition(deltaC);
            D.AddVelocityAndApplyToPosition(deltaD);
        }

        public static float GetGravityDiff(MoonSingleDimension a, MoonSingleDimension b)
        {
            var diff = b.Position - a.Position;
            return float.Clamp(diff, -1, 1);
        }
        public bool Equals(OrbitalSystemSingleDimension other) => A.Equals(other.A) && B.Equals(other.B) && C.Equals(other.C) && D.Equals(other.D);
    }

    public struct MoonSingleDimension : IEquatable<MoonSingleDimension>
    {
        public float Position;
        public float Velocity;

        public MoonSingleDimension(float position)
        {
            Position = position;
        }

        public void AddVelocityAndApplyToPosition(float velocityChange)
        {
            var newVelocity = Velocity + velocityChange;
            Position += newVelocity;
            Velocity = newVelocity;
        }

        public bool Equals(MoonSingleDimension other) => Position == other.Position && Velocity == other.Velocity;
    }
}
