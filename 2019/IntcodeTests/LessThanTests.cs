using Intcode.Instructions;
using System.Collections.Generic;
using System.Numerics;
using Xunit;

namespace Intcode.Tests
{
    public class LessThanTests
    {
        [Fact]
        public void LessThanTest1()
        {
            // Assemble
            var memory = new List<BigInteger> { 0, 1, 2, 0 };
            var lessThan = new LessThan(memory.CreateState(), ParameterMode.Immediate, ParameterMode.Immediate);

            // Act
            lessThan.Execute();

            // Assert
            var expected = new List<BigInteger> { 1, 1, 2, 0 };
            Assert.Equal(expected, memory);
        }
    }
}
