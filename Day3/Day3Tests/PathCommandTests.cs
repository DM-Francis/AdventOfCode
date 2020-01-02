using Day3_CrossedWires;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Day3Tests
{
    public class PathCommandTests
    {
        [Fact]
        public void SetsDirectionCorrectly()
        {
            // Assemble & Act
            var leftCommand = new PathCommand("L10");
            var rightCommand = new PathCommand("R10");
            var upCommand = new PathCommand("U10");
            var downCommand = new PathCommand("D10");

            // Assert
            Assert.Equal(Direction.Left, leftCommand.Direction);
            Assert.Equal(Direction.Right, rightCommand.Direction);
            Assert.Equal(Direction.Up, upCommand.Direction);
            Assert.Equal(Direction.Down, downCommand.Direction);
        }

        [Fact]
        public void ThrowsWhenCommandStringIsOnlyOneCharactor()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new PathCommand("U"));
        }
    }
}
