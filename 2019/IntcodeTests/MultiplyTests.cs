using Intcode.Instructions;
using System.Collections.Generic;
using System.Numerics;
using Xunit;

namespace Intcode.Tests
{
    public class MultiplyTests
    {
        [Fact]
        public void MultiplyPositionTest1()
        {
            // Assemble
            // Will multiply 10 to 20 and place the answer in the final slot
            var memory = new List<BigInteger> { 1, 4, 5, 6, 10, 20, 0 };
            var multiply = new Multiply(memory.CreateState(), ParameterMode.Position, ParameterMode.Position);

            // Act
            multiply.Execute();

            // Assert
            var expected = new List<BigInteger> { 1, 4, 5, 6, 10, 20, 200 };
            Assert.Equal(expected, memory);
        }

        [Fact]
        public void MultiplyImmediateTest()
        {
            // Assemble
            var memory = new List<BigInteger> { 1, 2, 3, 0 }; // 2 * 3
            var multiply = new Multiply(memory.CreateState(), ParameterMode.Immediate, ParameterMode.Immediate);

            // Act
            multiply.Execute();

            // Assert
            var expected = new List<BigInteger> { 6, 2, 3, 0 };
            Assert.Equal(expected, memory);
        }

        [Fact]
        public void MultiplyPositionAndImmediateTest()
        {
            // Assemble
            var memory = new List<BigInteger> { 1, 2, 4, 0, 10 }; // Will multiply 2 with 10
            var multiply = new Multiply(memory.CreateState(), ParameterMode.Immediate, ParameterMode.Position);

            // Act
            multiply.Execute();

            // Assert
            var expected = new List<BigInteger> { 20, 2, 4, 0, 10 };
            Assert.Equal(expected, memory);
        }
    }
}
