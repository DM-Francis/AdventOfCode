using Day5_SunnyWithAChanceOfAsteroids;
using Day5_SunnyWithAChanceOfAsteroids.Instructions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Day5Tests
{
    public class MultiplyTests
    {
        [Fact]
        public void MultiplyPositionTest1()
        {
            // Assemble
            // Will multiply 10 to 20 and place the answer in the final slot
            var memory = new List<int> { 1, 4, 5, 6, 10, 20, 0 };
            var multiply = new Multiply(ParameterMode.Position, ParameterMode.Position);

            // Act
            multiply.Execute(memory, 0, 0, null);

            // Assert
            var expected = new List<int> { 1, 4, 5, 6, 10, 20, 200 };
            Assert.Equal(expected, memory);
        }

        [Fact]
        public void MultiplyImmediateTest()
        {
            // Assemble
            var memory = new List<int> { 1, 2, 3, 0 }; // 2 * 3
            var multiply = new Multiply(ParameterMode.Immediate, ParameterMode.Immediate);

            // Act
            multiply.Execute(memory, 0, 0, null);

            // Assert
            var expected = new List<int> { 6, 2, 3, 0 };
            Assert.Equal(expected, memory);
        }

        [Fact]
        public void MultiplyPositionAndImmediateTest()
        {
            // Assemble
            var memory = new List<int> { 1, 2, 4, 0, 10 }; // Will multiply 2 with 10
            var multiply = new Multiply(ParameterMode.Immediate, ParameterMode.Position);

            // Act
            multiply.Execute(memory, 0, 0, null);

            // Assert
            var expected = new List<int> { 20, 2, 4, 0, 10 };
            Assert.Equal(expected, memory);
        }
    }
}
