using Intcode;
using System;
using System.Collections.Generic;
using Xunit;

namespace Intcode.Tests
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
            Assert.Equal(expected, interpreter.Memory);
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
            Assert.Equal(expected, interpreter.Memory);
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
            Assert.Equal(expected, interpreter.Memory);
        }

        [Fact]
        public void InputOperationTest1()
        {
            // Assemble
            var program = new List<int> { 3, 0, 99 };
            var interpreter = new IntcodeInterpreter(program);

            // Act
            interpreter.Interpret(10);

            // Assert
            var expected = new List<int> { 10, 0, 99 };
            Assert.Equal(expected, interpreter.Memory);
        }

        [Fact]
        public void OutputOperationTest1()
        {
            // Assemble
            var program = new List<int> { 4, 3, 99, 40 };
            var output = new List<int>();
            var interpreter = new IntcodeInterpreter(program, o => output.Add(o));

            // Act
            interpreter.Interpret();

            // Assert
            var expectedMemory = new List<int> { 4, 3, 99, 40 };
            var expectedOutput = new List<int> { 40 };
            Assert.Equal(expectedMemory, interpreter.Memory);
            Assert.Equal(expectedOutput, output);
        }

        [Fact]
        public void ComparisonTest_EqualTo8True()
        {
            // Assemble
            var program = new List<int> { 3, 9, 8, 9, 10, 9, 4, 9, 99, -1, 8 };
            var output = new List<int>();
            var interpreter = new IntcodeInterpreter(program, o => output.Add(o));

            // Act
            interpreter.Interpret(8);

            // Assert
            var expectedOutput = new List<int> { 1 };
            Assert.Equal(expectedOutput, output);
        }

        [Fact]
        public void ComparisonTest_EqualTo8False()
        {
            // Assemble
            var program = new List<int> { 3, 9, 8, 9, 10, 9, 4, 9, 99, -1, 8 };
            var output = new List<int>();
            var interpreter = new IntcodeInterpreter(program, o => output.Add(o));

            // Act
            interpreter.Interpret(5);

            // Assert
            var expectedOutput = new List<int> { 0 };
            Assert.Equal(expectedOutput, output);
        }

        [Fact]
        public void ComparisonTest_LessThan8True()
        {
            // Assemble
            var program = new List<int> { 3, 3, 1107, -1, 8, 3, 4, 3, 99 };
            var output = new List<int>();
            var interpreter = new IntcodeInterpreter(program, o => output.Add(o));

            // Act
            interpreter.Interpret(2);

            // Assert
            var expectedOutput = new List<int> { 1 };
            Assert.Equal(expectedOutput, output);
        }

        [Fact]
        public void ComparisonTest_LessThan8False()
        {
            // Assemble
            var program = new List<int> { 3, 3, 1107, -1, 8, 3, 4, 3, 99 };
            var output = new List<int>();
            var interpreter = new IntcodeInterpreter(program, o => output.Add(o));

            // Act
            interpreter.Interpret(16);

            // Assert
            var expectedOutput = new List<int> { 0 };
            Assert.Equal(expectedOutput, output);
        }

        [Fact]
        public void JumpTestNonZero()
        {
            // Assemble
            var program = new List<int> { 3, 12, 6, 12, 15, 1, 13, 14, 13, 4, 13, 99, -1, 0, 1, 9 };
            var output = new List<int>();
            var interpreter = new IntcodeInterpreter(program, o => output.Add(o));

            // Act
            interpreter.Interpret(-9);

            // Assert
            var expectedOutput = new List<int> { 1 };
            Assert.Equal(expectedOutput, output);
        }

        [Fact]
        public void ComparisonTo8_GreaterThan()
        {
            // Assemble
            var program = new List<int> { 3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99 };
            var output = new List<int>();
            var interpreter = new IntcodeInterpreter(program, o => output.Add(o));

            // Act
            interpreter.Interpret(20);

            // Assert
            var expectedOutput = new List<int> { 1001 };
            Assert.Equal(expectedOutput, output);
        }

        [Fact]
        public void ComparisonTo8_EqualTo()
        {
            // Assemble
            var program = new List<int> { 3, 21, 1008, 21, 8, 20, 1005, 20, 22, 107, 8, 21, 20, 1006, 20, 31, 1106, 0, 36, 98, 0, 0, 1002, 21, 125, 20, 4, 20, 1105, 1, 46, 104, 999, 1105, 1, 46, 1101, 1000, 1, 20, 4, 20, 1105, 1, 46, 98, 99 };
            var output = new List<int>();
            var interpreter = new IntcodeInterpreter(program, o => output.Add(o));

            // Act
            interpreter.Interpret(8);

            // Assert
            var expectedOutput = new List<int> { 1000 };
            Assert.Equal(expectedOutput, output);
        }

        [Fact]
        public void ComparisonTo8_LessThan()
        {
            // Assemble
            var program = new List<int> { 3, 21, 1008, 21, 8, 20, 1005, 20, 22, 107, 8, 21, 20, 1006, 20, 31, 1106, 0, 36, 98, 0, 0, 1002, 21, 125, 20, 4, 20, 1105, 1, 46, 104, 999, 1105, 1, 46, 1101, 1000, 1, 20, 4, 20, 1105, 1, 46, 98, 99 };
            var output = new List<int>();
            var interpreter = new IntcodeInterpreter(program, o => output.Add(o));

            // Act
            interpreter.Interpret(2);

            // Assert
            var expectedOutput = new List<int> { 999 };
            Assert.Equal(expectedOutput, output);
        }
    }
}
