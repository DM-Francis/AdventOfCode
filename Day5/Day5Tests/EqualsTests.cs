using Day5_SunnyWithAChanceOfAsteroids;
using Day5_SunnyWithAChanceOfAsteroids.Instructions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Day5Tests
{
    public class EqualsTests
    {
        [Fact]
        public void EqualsTest1()
        {
            // Assemble
            var memory = new List<int> { 0, 2, 2, 0 };
            var equals = new Equals(ParameterMode.Immediate, ParameterMode.Immediate);

            // Act
            equals.Execute(memory, 0, 0, null);

            // Assert
            var expected = new List<int> { 1, 2, 2, 0 };
            Assert.Equal(expected, memory);
        }

        [Fact]
        public void EqualsTest2()
        {
            // Assemble
            var memory = new List<int> { 0, 4, 2, 0, 2 };       // Comparing the last digit (2) to the second (2)
            var equals = new Equals(ParameterMode.Position, ParameterMode.Immediate);

            // Act
            equals.Execute(memory, 0, 0, null);

            // Assert
            var expected = new List<int> { 1, 4, 2, 0, 2 };
            Assert.Equal(expected, memory);
        }
    }
}
