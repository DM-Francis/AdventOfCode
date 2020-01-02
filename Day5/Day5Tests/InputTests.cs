using Day5_SunnyWithAChanceOfAsteroids.Instructions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Day5Tests
{
    public class InputTests
    {
        [Fact]
        public void InputTest()
        {
            // Assemble
            var memory = new List<int> { 3, 2, 0 };
            var inputValue = 10;
            var input = new Input();

            // Act
            input.Execute(memory, 0, inputValue, null);

            // Assert
            var expected = new List<int> { 3, 2, 10 };
            Assert.Equal(expected, memory);
        }
    }
}
