using System;
using System.Collections.Generic;
using System.Text;

namespace Day6_UniversalOrbitMap
{
    public class SpaceObject
    {
        private int _subOrbitCount;
        private bool _subOrbitCountCached = false;

        public string Name { get; }
        public SpaceObject? Parent { get; set; }

        public SpaceObject(string name, SpaceObject? parent)
        {
            Name = name;
            Parent = parent;
        }

        public int CountSubOrbits()
        {
            if (_subOrbitCountCached)
            {
                return _subOrbitCount;
            }

            if (Parent == null)
            {
                _subOrbitCount = 0;
            }
            else
            {
                _subOrbitCount = Parent.CountSubOrbits() + 1;
            }

            _subOrbitCountCached = true;
            return _subOrbitCount;
        }

        public List<SpaceObject> GetSubOrbits()
        {
            if (Parent == null)
            {
                return new List<SpaceObject>();
            }
            else
            {
                var subOrbits = Parent.GetSubOrbits();
                subOrbits.Add(Parent);
                return subOrbits;
            }
        }

        public void ResetCache()
        {
            _subOrbitCountCached = false;
        }
    }
}
