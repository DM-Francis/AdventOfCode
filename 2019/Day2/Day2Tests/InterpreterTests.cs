using Day2_1202ProgramAlarm;
using System;
using System.Collections.Generic;
using Xunit;

namespace Day2Tests
{
    public class InterpreterTests
    {
        [Fact]
        public void BasicProgram1()
        {
            // Assemble
            var program = new List<int> { 1, 0, 0, 0, 99 };
            var interpreter = new IntcodeInterpreter(program);

            // Act
            interpreter.Interpret();

            // Assert
            var expected = new List<int> { 2, 0, 0, 0, 99 };
            Assert.Equal(expected, interpreter.State);
        }

        [Fact]
        public void BasicProgram2()
        {
            // Assemble
            var program = new List<int> { 2, 3, 0, 3, 99 };
            var interpreter = new IntcodeInterpreter(program);

            // Act
            interpreter.Interpret();

            // Assert
            var expected = new List<int> { 2, 3, 0, 6, 99 };
            Assert.Equal(expected, interpreter.State);
        }

        [Fact]
        public void BasicProgram3()
        {
            // Assemble
            var program = new List<int> { 2, 4, 4, 5, 99, 0 };
            var interpreter = new IntcodeInterpreter(program);

            // Act
            interpreter.Interpret();

            // Assert
            var expected = new List<int> { 2, 4, 4, 5, 99, 9801 };
            Assert.Equal(expected, interpreter.State);
        }
    }
}
