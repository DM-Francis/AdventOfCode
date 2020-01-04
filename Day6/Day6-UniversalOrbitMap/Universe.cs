using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Day6_UniversalOrbitMap
{
    public class Universe
    {
        private readonly Dictionary<string, SpaceObject> _spaceObjects = new Dictionary<string, SpaceObject>();

        public Universe()
        {
            _spaceObjects["COM"] = new SpaceObject("COM", null);
        }

        public void AddRawSpaceObject(string rawOrbit)
        {
            int orbitCharIndex = rawOrbit.IndexOf(')');

            string parentName = rawOrbit.Substring(0, orbitCharIndex);
            string name = rawOrbit.Substring(orbitCharIndex + 1);

            AddSpaceObject(name, parentName);
        }

        public void AddSpaceObject(string name, string parentName)
        {
            if (string.IsNullOrEmpty(parentName))
            {
                throw new ArgumentException($"Parent name cannot be null or empty", nameof(parentName));
            }

            AddIfNotExists(name);
            AddIfNotExists(parentName);
            SetParent(name, parentName);
        }

        private void AddIfNotExists(string name)
        {
            _spaceObjects.TryAdd(name, new SpaceObject(name, null));
        }

        private void SetParent(string name, string parentName)
        {
            var parent = _spaceObjects[parentName];
            _spaceObjects[name].Parent = parent;
        }

        public int CountOrbits()
        {
            int count = 0;
            foreach(var spaceObject in _spaceObjects.Values)
            {
                count += spaceObject.CountSubOrbits();
            }

            return count;
        }

        public int GetDistanceBetween(string name1, string name2)
        {
            return GetDistanceBetween(_spaceObjects[name1], _spaceObjects[name2]);
        }

        private int GetDistanceBetween(SpaceObject object1, SpaceObject object2)
        {
            var orbits1 = object1.GetSubOrbits();
            var orbits2 = object2.GetSubOrbits();

            orbits1.Reverse();
            orbits2.Reverse();

            var interception = (from o1 in orbits1
                                from o2 in orbits2
                                where o1 == o2
                                select o1).First();

            return orbits1.IndexOf(interception) + orbits2.IndexOf(interception);
        }
    }
}
