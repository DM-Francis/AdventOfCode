using Day10_MonitoringStation;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Day10Tests
{
    public class AsteroidTests
    {
        [Fact]
        public void GetAngleViewedFromTest1()
        {
            // Assemble
            var universe = new Universe(2, 2);
            universe.AddAsteroidAt(0, 0);
            universe.AddAsteroidAt(1, 1);

            // Act
            var ast = universe.GetAsteroidAt(1, 1);
            double angle = ast.GetAngleAsViewedFrom(0, 0);

            // Assert
            Assert.Equal(135, angle);
        }

        [Fact]
        public void GetAngleViewedFromTest2()
        {
            // Assemble
            var universe = new Universe(2, 2);
            universe.AddAsteroidAt(0, 0);
            universe.AddAsteroidAt(1, 0);

            // Act
            var ast = universe.GetAsteroidAt(1, 0);
            double angle = ast.GetAngleAsViewedFrom(0, 0);

            // Assert
            Assert.Equal(90, angle);
        }

        [Fact]
        public void GetAngleViewedFromTest3()
        {
            // Assemble
            var universe = new Universe(3, 3);
            universe.AddAsteroidAt(0, 0);
            universe.AddAsteroidAt(1, 2);

            // Act
            var ast = universe.GetAsteroidAt(1, 2);
            double angle = ast.GetAngleAsViewedFrom(0, 0);

            // Assert
            Assert.Equal(153, angle, 0);
        }

        [Fact]
        public void GetAngleViewedFromTest4()
        {
            // Assemble
            var universe = new Universe(2, 2);
            universe.AddAsteroidAt(0, 0);
            universe.AddAsteroidAt(1, 1);

            // Act
            var ast = universe.GetAsteroidAt(0, 0);
            double angle = ast.GetAngleAsViewedFrom(1, 1);

            // Assert
            Assert.Equal(315, angle);
        }
    }
}
