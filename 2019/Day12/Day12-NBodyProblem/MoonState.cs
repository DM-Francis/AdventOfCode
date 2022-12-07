using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Day12_NBodyProblem
{
    public class MoonState : IEnumerable<Moon>
    {
        private readonly List<Moon> _moons;
        private float _totalPotentialEnergy;
        private float _totalKineticEnergy;

        public Moon this[int i]
        {
            get => _moons[i];
            set
            {
                _totalPotentialEnergy -= _moons[i].PotentialEnergy;
                _totalKineticEnergy -= _moons[i].KineticEnergy;
                _moons[i] = value;
                _totalPotentialEnergy += value.PotentialEnergy;
                _totalKineticEnergy += value.KineticEnergy;
            }
        }

        public int Count => _moons.Count;

        public float TotalPotentialEnergy => _totalPotentialEnergy;
        public float TotalKineticEnergy => _totalKineticEnergy;

        public MoonState(IEnumerable<Moon> moons)
        {
            _moons = new List<Moon>(moons);
            _totalPotentialEnergy = _moons.Sum(m => m.PotentialEnergy);
            _totalKineticEnergy = _moons.Sum(m => m.KineticEnergy);
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
