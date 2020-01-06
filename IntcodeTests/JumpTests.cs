using Intcode.Instructions;
using System.Collections.Generic;
using Xunit;

namespace Intcode.Tests
{
    public class JumpTests
    {
        [Fact]
        public void JumpIfTruePositionTest1()
        {
            // Assemble
            var memory = new List<int> { 0, 3, 4, 100, 1 }; // Pointer should jump to position 1
            var jumpIfTrue = new JumpIfTrue(ParameterMode.Position, ParameterMode.Position);

            // Act
            int newPosition = jumpIfTrue.Execute(memory, 0, 0, null);

            // Assert
            Assert.Equal(1, newPosition);
        }

        [Fact]
        public void JumpIfTruePositionTest2()
        {
            // Assemble
            var memory = new List<int> { 0, 3, 4, 0, 1 }; // Pointer should move forward
            var jumpIfTrue = new JumpIfTrue(ParameterMode.Position, ParameterMode.Position);

            // Act
            int newPosition = jumpIfTrue.Execute(memory, 0, 0, null);

            // Assert
            Assert.Equal(3, newPosition);
        }

        [Fact]
        public void JumpIfFalsePositionTest1()
        {
            // Assemble
            var memory = new List<int> { 0, 3, 4, 0, 1 }; // Pointer should jump to position 1
            var jumpIfFalse = new JumpIfFalse(ParameterMode.Position, ParameterMode.Position);

            // Act
            int newPosition = jumpIfFalse.Execute(memory, 0, 0, null);

            // Assert
            Assert.Equal(1, newPosition);
        }

        [Fact]
        public void JumpIfFalsePositionTest2()
        {
            // Assemble
            var memory = new List<int> { 0, 3, 4, 100, 1 }; // Pointer should move forward
            var jumpIfFalse = new JumpIfFalse(ParameterMode.Position, ParameterMode.Position);

            // Act
            int newPosition = jumpIfFalse.Execute(memory, 0, 0, null);

            // Assert
            Assert.Equal(3, newPosition);
        }
    }
}
