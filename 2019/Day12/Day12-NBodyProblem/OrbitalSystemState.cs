using System;
using System.Collections.Generic;
using System.Numerics;

namespace Day12_NBodyProblem
{
    public struct OrbitalSystemState : IEquatable<OrbitalSystemState>
    {
        public Moon A;
        public Moon B;
        public Moon C;
        public Moon D;

        public OrbitalSystemState(IReadOnlyList<Moon> moons)
        {
            if (moons.Count != 4) throw new ArgumentException("Must provide exactly 4 moons");

            A = moons[0];
            B = moons[1];
            C = moons[2];
            D = moons[3];
        }

        public OrbitalSystemState(Moon a, Moon b, Moon c, Moon d)
        {
            A = a; B = b; C = c; D = d;
        }

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

        public float PotentialEnergy => A.PotentialEnergy + B.PotentialEnergy + C.PotentialEnergy + D.PotentialEnergy;
        public float KineticEnergy => A.KineticEnergy + B.KineticEnergy + C.KineticEnergy + D.KineticEnergy;
        public float TotalEnergy => A.TotalEnergy + B.TotalEnergy + C.TotalEnergy + D.TotalEnergy;

        private static Vector3 GetGravityDiff(Moon moonA, Moon moonB)
        {
            var diff = moonB.Position - moonA.Position;
            return Vector3.Clamp(diff, -Vector3.One, Vector3.One);
        }

        public bool Equals(OrbitalSystemState other)
        {
            return A.Equals(other.A) && B.Equals(other.B) && C.Equals(other.C) && D.Equals(other.D);
        }
    }
}
