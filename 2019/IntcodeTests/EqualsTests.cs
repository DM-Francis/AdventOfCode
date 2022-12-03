using Intcode.Instructions;
using System.Collections.Generic;
using System.Numerics;
using Xunit;

namespace Intcode.Tests
{
    public class EqualsTests
    {
        [Fact]
        public void EqualsTest1()
        {
            // Assemble
            var memory = new List<BigInteger> { 0, 2, 2, 0 };
            var equals = new Equals(memory.CreateState(), ParameterMode.Immediate, ParameterMode.Immediate);

            // Act
            equals.Execute();

            // Assert
            var expected = new List<BigInteger> { 1, 2, 2, 0 };
            Assert.Equal(expected, memory);
        }

        [Fact]
        public void EqualsTest2()
        {
            // Assemble
            var memory = new List<BigInteger> { 0, 4, 2, 0, 2 };       // Comparing the last digit (2) to the second (2)
            var equals = new Equals(memory.CreateState(), ParameterMode.Position, ParameterMode.Immediate);

            // Act
            equals.Execute();

            // Assert
            var expected = new List<BigInteger> { 1, 4, 2, 0, 2 };
            Assert.Equal(expected, memory);
        }
    }
}
