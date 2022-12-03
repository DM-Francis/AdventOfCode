using Day5_SunnyWithAChanceOfAsteroids;
using Day5_SunnyWithAChanceOfAsteroids.Instructions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Day5Tests
{
    public class InstructionFactoryTests
    {
        [Fact]
        public void CreatesCorrectInstructionObjects_Test1()
        {
            // Assemble & Act
            var add = InstructionFactory.Get(1);
            var multiply = InstructionFactory.Get(2);
            var input = InstructionFactory.Get(3);
            var output = InstructionFactory.Get(4);
            var halt = InstructionFactory.Get(99);

            // Assert
            Assert.IsType<Add>(add);
            Assert.IsType<Multiply>(multiply);
            Assert.IsType<Input>(input);
            Assert.IsType<Output>(output);
            Assert.IsType<Halt>(halt);
        }

        [Fact]
        public void CreatesCorrectInstructionObjects_Test2()
        {
            // Assemble & Act
            var jumpIfTrue = InstructionFactory.Get(5);
            var jumpIfFalse = InstructionFactory.Get(6);
            var lessThan = InstructionFactory.Get(7);
            var equals = InstructionFactory.Get(8);

            // Assert
            Assert.IsType<JumpIfTrue>(jumpIfTrue);
            Assert.IsType<JumpIfFalse>(jumpIfFalse);
            Assert.IsType<LessThan>(lessThan);
            Assert.IsType<Equals>(equals);
        }

        [Fact]
        public void SetsCorrectParameterModes1()
        {
            // Assemble & Act
            var add = InstructionFactory.Get(1) as Add;

            // Assert
            Assert.Equal(ParameterMode.Position, add.Param1Mode);
            Assert.Equal(ParameterMode.Position, add.Param2Mode);
        }

        [Fact]
        public void SetsCorrectParameterModes2()
        {
            // Assemble & Act
            var add = InstructionFactory.Get(1001) as Add;

            // Assert
            Assert.Equal(ParameterMode.Position, add.Param1Mode);
            Assert.Equal(ParameterMode.Immediate, add.Param2Mode);
        }

        [Fact]
        public void SetsCorrectParameterModes3()
        {
            // Assemble & Act
            var output = InstructionFactory.Get(104) as Output;

            // Assert
            Assert.Equal(ParameterMode.Immediate, output.Param1Mode);
        }

        [Fact]
        public void CreatesCorrectInstructionObjects_Test3()
        {
            // Assemble & Act
            var add = InstructionFactory.Get(1001);
            var multiply = InstructionFactory.Get(1102);
            var input = InstructionFactory.Get(103);
            var output = InstructionFactory.Get(04);
            var halt = InstructionFactory.Get(99);

            // Assert
            Assert.IsType<Add>(add);
            Assert.IsType<Multiply>(multiply);
            Assert.IsType<Input>(input);
            Assert.IsType<Output>(output);
            Assert.IsType<Halt>(halt);
        }
    }
}
