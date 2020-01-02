using Day3_CrossedWires;
using System;
using Xunit;

namespace Day3Tests
{
    public class PointTests
    {
        [Fact]
        public void LeftOfTest()
        {
            // Assemble
            var start = new Point(2, 3);

            // Act
            Point newPoint = Point.LeftOf(start, 5);

            // Assert
            Assert.Equal(-3, newPoint.X);
            Assert.Equal(3, newPoint.Y);
        }

        [Fact]
        public void RightOfTest()
        {
            // Assemble
            var start = new Point(2, 3);

            // Act
            Point newPoint = Point.RightOf(start, 6);

            // Assert
            Assert.Equal(8, newPoint.X);
            Assert.Equal(3, newPoint.Y);
        }

        [Fact]
        public void UpFromTest()
        {
            // Assemble
            var start = new Point(2, 3);

            // Act
            Point newPoint = Point.UpFrom(start, 3);

            // Assert
            Assert.Equal(2, newPoint.X);
            Assert.Equal(6, newPoint.Y);
        }

        [Fact]
        public void DownFromTest()
        {
            // Assemble
            var start = new Point(2, 3);

            // Act
            Point newPoint = Point.DownFrom(start, 7);

            // Assert
            Assert.Equal(2, newPoint.X);
            Assert.Equal(-4, newPoint.Y);
        }
    }
}
