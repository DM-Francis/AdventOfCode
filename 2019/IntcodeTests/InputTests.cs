using Intcode.Instructions;
using System.Collections.Generic;
using System.Numerics;
using Xunit;

namespace Intcode.Tests
{
    public class InputTests
    {
        [Fact]
        public void InputTest()
        {
            // Assemble
            var memory = new List<BigInteger> { 3, 2, 0 };
            var state = memory.CreateState();
            state.InputValue = 10;
            var input = new Input(state);

            // Act
            input.Execute();

            // Assert
            var expected = new List<BigInteger> { 3, 2, 10 };
            Assert.Equal(expected, memory);
        }
    }
}
