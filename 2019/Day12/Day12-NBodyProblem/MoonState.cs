using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Day12_NBodyProblem
{
    public class MoonState : IEquatable<MoonState>, IEnumerable<Moon>
    {
        private readonly List<Moon> _moons;

        public Moon this[int i] => _moons[i];
        public int Count => _moons.Count;

        public MoonState(IEnumerable<Moon> moons)
        {
            _moons = new List<Moon>(moons.Select(m => m.GetCopy()));
        }

        public MoonState GetCopy()
        {
            return new MoonState(this);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as MoonState);
        }

        public bool Equals([AllowNull] MoonState other)
        {
            if (other == null) return false;
            if (Count != other.Count) return false;

            for (int i = 0; i < Count; i++)
            {
                if (!this[i].Equals(other[i])) return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            int currentHash = this[0].GetHashCode();

            for (int i = 1; i < Count; i++)
            {
                currentHash = HashCode.Combine(currentHash, this[i]);
            }

            return currentHash;
        }

        public IEnumerator<Moon> GetEnumerator()
        {
            return ((IEnumerable<Moon>)_moons).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<Moon>)_moons).GetEnumerator();
        }
    }
}
