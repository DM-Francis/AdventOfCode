using Intcode.Instructions;
using System.Collections.Generic;
using System.Numerics;
using Xunit;

namespace Intcode.Tests
{
    public class OutputTests
    {
        [Fact]
        public void OutputPositionTest()
        {
            // Assemble
            var memory = new List<BigInteger> { 4, 2, 20 };
            var state = memory.CreateState();
            var output = new Output(state, ParameterMode.Position);

            // Act
            output.Execute();

            // Assert
            var expected = 20;
            Assert.Equal(expected, state.OutputValue);
        }

        [Fact]
        public void OutputImmediateTest()
        {
            // Assemble
            var memory = new List<BigInteger> { 4, 15, 20 };
            var state = memory.CreateState();
            var output = new Output(state, ParameterMode.Immediate);

            // Act
            output.Execute();

            // Assert
            var expected = 15;
            Assert.Equal(expected, state.OutputValue);
        }
    }
}
