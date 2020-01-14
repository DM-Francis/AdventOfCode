using Day10_MonitoringStation;
using System;
using Xunit;

namespace Day10Tests
{
    public class UniverseTests
    {
        [Fact]
        public void UniverseWith2Asteroids()
        {
            // Assemble
            var universe = new Universe(2, 2);
            universe.AddAsteroidAt(0, 0);
            universe.AddAsteroidAt(1, 0);

            // Act
            var ast = universe.GetAsteroidAt(0, 0);
            bool canSee = ast.CanSee(1, 0);

            // Assert
            Assert.True(canSee);
        }

        [Fact]
        public void MoreComplexUniverse1()
        {
            // Assemble
            var universe = new Universe(5, 5);
            universe.AddAsteroidAt(1, 0);
            universe.AddAsteroidAt(4, 0);
            universe.AddAsteroidAt(0, 2);
            universe.AddAsteroidAt(1, 2);
            universe.AddAsteroidAt(2, 2);
            universe.AddAsteroidAt(3, 2);
            universe.AddAsteroidAt(4, 2);
            universe.AddAsteroidAt(4, 3);
            universe.AddAsteroidAt(3, 4);
            universe.AddAsteroidAt(4, 4);

            // Act
            var ast = universe.GetAsteroidAt(3, 4);

            // Assert
            Assert.False(ast.CanSee(1, 0));
            Assert.True(ast.CanSee(4, 0));
        }
    }
}
