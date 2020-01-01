using Day3_CrossedWires;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Day3Tests
{
    public class WirePathTests
    {
        [Fact]
        public void PathStartsAtOrigin()
        {
            // Assemble & Act
            var path = new WirePath();
            List<Point> result = path.GetPoints();

            // Assert
            Assert.Collection(result, p => Assert.Equal(new Point(0,0), p));
        }

        [Fact]
        public void GetPointsReturnsValuesInCorrectOrder()
        {
            // Assemble
            var path = new WirePath();
            path.Add("U1");

            // Act
            List<Point> result = path.GetPoints();

            // Assert
            Assert.Collection(result,
                p1 => Assert.Equal(new Point(0, 0), p1),
                p2 => Assert.Equal(new Point(0, 1), p2));
        }

        [Fact]
        public void MinMaxMethodsWorkCorrectly()
        {
            // Assemble
            var path = new WirePath();
            path.Add("L1");                     // .2-3.
            path.Add("U2");                     // .|.|.
            path.Add("R2");                     // .10|.
            path.Add("D4");                     // ...|.
                                                // ...4.

            // Act
            int minX = path.GetMinX();
            int maxX = path.GetMaxX();
            int minY = path.GetMinY();
            int maxY = path.GetMaxY();

            // Assert
            Assert.Equal(-1, minX);
            Assert.Equal(1, maxX);
            Assert.Equal(-2, minY);
            Assert.Equal(2, maxY);
        }
    }
}
