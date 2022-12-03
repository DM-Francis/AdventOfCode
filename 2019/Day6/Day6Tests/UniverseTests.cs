using Day6_UniversalOrbitMap;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Day6Tests
{
    public class UniverseTests
    {
        [Fact]
        public void SmallUniverseTestCount()
        {
            // Assemble
            var orbitsRaw = new List<string> { "COM)B",
                                                 "B)C",
                                                 "C)D",
                                                 "D)E",
                                                 "E)F",
                                                 "B)G",
                                                 "G)H",
                                                 "D)I",
                                                 "E)J",
                                                 "J)K",
                                                 "K)L"};

            var universe = new Universe();            

            // Act
            foreach(var o in orbitsRaw) { universe.AddRawSpaceObject(o); }
            int orbitCount = universe.CountOrbits();

            // Assert
            Assert.Equal(42, orbitCount);
        }

        [Fact]
        public void RawOrbitsCanBeLoadedInAnyOrder()
        {
            // Assemble
            var orbitsRaw = new List<string> {
                                                 "E)F",
                                                 "B)C",
                                                 "D)E",
                                                 "COM)B",
                                                 "D)I",
                                                 "C)D",
                                                 "B)G",
                                                 "J)K",
                                                 "G)H",
                                                 "K)L",
                                                 "E)J"};

            var universe = new Universe();

            // Act
            foreach (var o in orbitsRaw) { universe.AddRawSpaceObject(o); }
            int orbitCount = universe.CountOrbits();

            // Assert
            Assert.Equal(42, orbitCount);
        }

        [Fact]
        public void GetDistanceBetweenObjectsIsCorrect()
        {
            // Assemble
            var orbitsRaw = new List<string> {
                                                "COM)B",
                                                "B)C",
                                                "C)D",
                                                "D)E",
                                                "E)F",
                                                "B)G",
                                                "G)H",
                                                "D)I",
                                                "E)J",
                                                "J)K",
                                                "K)L",
                                                "K)YOU",
                                                "I)SAN"};

            var universe = new Universe();
            foreach (var o in orbitsRaw) { universe.AddRawSpaceObject(o); }

            // Act
            int distanceBetween = universe.GetDistanceBetween("YOU", "SAN");

            // Assert
            Assert.Equal(4, distanceBetween);
        }
    }
}
