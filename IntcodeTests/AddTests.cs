using Intcode.Instructions;
using System.Collections.Generic;
using Xunit;

namespace Intcode.Tests
{
    public class AddTests
    {
        [Fact]
        public void AddPositionTest1()
        {
            // Assemble
            // Will add 10 to 20 and place the answer in the final slot
            var memory = new List<int> { 1, 4, 5, 6, 10, 20, 0 };
            var add = new Add(ParameterMode.Position, ParameterMode.Position);

            // Act
            add.Execute(memory, 0, 0, null);

            // Assert
            var expected = new List<int> { 1, 4, 5, 6, 10, 20, 30 };
            Assert.Equal(expected, memory);
        }

        [Fact]
        public void AddImmediateTest()
        {
            // Assemble
            var memory = new List<int> { 1, 2, 3, 0 };
            var add = new Add(ParameterMode.Immediate, ParameterMode.Immediate);

            // Act
            add.Execute(memory, 0, 0, null);

            // Assert
            var expected = new List<int> { 5, 2, 3, 0 };
            Assert.Equal(expected, memory);
        }

        [Fact]
        public void AddPositionAndImmediateTest()
        {
            // Assemble
            var memory = new List<int> { 1, 2, 4, 0, 10 }; // Will add 2 to 10
            var add = new Add(ParameterMode.Immediate, ParameterMode.Position);

            // Act
            add.Execute(memory, 0, 0, null);

            // Assert
            var expected = new List<int> { 12, 2, 4, 0, 10 };
            Assert.Equal(expected, memory);
        }
    }
}
