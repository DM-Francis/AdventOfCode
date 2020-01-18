using Day12_NBodyProblem;
using System;
using System.Collections.Generic;
using System.Numerics;
using Xunit;

namespace Day12Tests
{
    public class OrbitalSystemTests
    {
        [Fact]
        public void Advance1TimeStep()
        {
            // Assemble
            var moons = new List<Moon> {
                new Moon(new Vector3(-1, 0, 2)),
                new Moon(new Vector3(2, -10, -7)),
                new Moon(new Vector3(4, -8, 8)),
                new Moon(new Vector3(3, 5, -1))
            };

            var system = new OrbitalSystem(moons);

            // Act
            system.AdvanceTimeStep();

            // Assert positions
            var moonState = system.Moons;

            Assert.Equal(new Vector3(2, -1, 1), moonState[0].Position);
            Assert.Equal(new Vector3(3, -7, -4), moonState[1].Position);
            Assert.Equal(new Vector3(1, -7, 5), moonState[2].Position);
            Assert.Equal(new Vector3(2, 2, 0), moonState[3].Position);

            // Assert velocities
            Assert.Equal(new Vector3(3, -1, -1), moonState[0].Velocity);
            Assert.Equal(new Vector3(1, 3, 3), moonState[1].Velocity);
            Assert.Equal(new Vector3(-3, 1, -3), moonState[2].Velocity);
            Assert.Equal(new Vector3(-1, -3, 1), moonState[3].Velocity);
        }

        [Fact]
        public void AdvanceMultipleTimeSteps()
        {
            // Assemble
            var moons = new List<Moon> {
                new Moon(new Vector3(-1, 0, 2)),
                new Moon(new Vector3(2, -10, -7)),
                new Moon(new Vector3(4, -8, 8)),
                new Moon(new Vector3(3, 5, -1))
            };

            var system = new OrbitalSystem(moons);

            // Act
            system.AdvanceTimeSteps(10);

            // Assert positions
            var moonState = system.Moons;
            Assert.Equal(new Vector3(2, 1, -3), moonState[0].Position);
            Assert.Equal(new Vector3(1, -8, 0), moonState[1].Position);
            Assert.Equal(new Vector3(3, -6, 1), moonState[2].Position);
            Assert.Equal(new Vector3(2, 0, 4), moonState[3].Position);

            // Assert velocities
            Assert.Equal(new Vector3(-3, -2, 1), moonState[0].Velocity);
            Assert.Equal(new Vector3(-1, 1, 3), moonState[1].Velocity);
            Assert.Equal(new Vector3(3, 2, -3), moonState[2].Velocity);
            Assert.Equal(new Vector3(1, -1, -1), moonState[3].Velocity);
        }
    }
}
