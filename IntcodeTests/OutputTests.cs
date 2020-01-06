using Intcode.Instructions;
using System.Collections.Generic;
using Xunit;

namespace Intcode.Tests
{
    public class OutputTests
    {
        [Fact]
        public void OutputPositionTest()
        {
            // Assemble
            var memory = new List<int> { 4, 2, 20 };
            var outputList = new List<int>();
            var output = new Output(ParameterMode.Position);

            // Act
            output.Execute(memory, 0, 0, v => outputList.Add(v));

            // Assert
            var expected = new List<int> { 20 };
            Assert.Equal(expected, outputList);
        }

        [Fact]
        public void OutputImmediateTest()
        {
            // Assemble
            var memory = new List<int> { 4, 15, 20 };
            var outputList = new List<int>();
            var output = new Output(ParameterMode.Immediate);

            // Act
            output.Execute(memory, 0, 0, v => outputList.Add(v));

            // Assert
            var expected = new List<int> { 15 };
            Assert.Equal(expected, outputList);
        }
    }
}
