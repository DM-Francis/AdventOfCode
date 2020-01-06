using Intcode.Instructions;
using System.Collections.Generic;
using Xunit;

namespace Intcode.Tests
{
    public class LessThanTests
    {
        [Fact]
        public void LessThanTest1()
        {
            // Assemble
            var memory = new List<int> { 0, 1, 2, 0 };
            var lessThan = new LessThan(ParameterMode.Immediate, ParameterMode.Immediate);

            // Act
            lessThan.Execute(memory, 0, 0, null);

            // Assert
            var expected = new List<int> { 1, 1, 2, 0 };
            Assert.Equal(expected, memory);
        }
    }
}
