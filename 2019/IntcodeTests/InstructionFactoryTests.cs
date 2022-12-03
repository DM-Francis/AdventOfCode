using Intcode.Instructions;
using Xunit;

namespace Intcode.Tests
{
    public class InstructionFactoryTests
    {
        private InstructionFactory CreateIntructionFactory()
        {
            return new InstructionFactory(new StubIntcodeState());
        }

        [Fact]
        public void CreatesCorrectInstructionObjects_Test1()
        {
            // Assemble & Act
            var factory = CreateIntructionFactory();

            var add = factory.Get(1);
            var multiply = factory.Get(2);
            var input = factory.Get(3);
            var output = factory.Get(4);
            var halt = factory.Get(99);

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
            var factory = CreateIntructionFactory();

            var jumpIfTrue = factory.Get(5);
            var jumpIfFalse = factory.Get(6);
            var lessThan = factory.Get(7);
            var equals = factory.Get(8);

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
            var factory = CreateIntructionFactory();
            var add = factory.Get(1) as Add;

            // Assert
            Assert.Equal(ParameterMode.Position, add.Param1Mode);
            Assert.Equal(ParameterMode.Position, add.Param2Mode);
        }

        [Fact]
        public void SetsCorrectParameterModes2()
        {
            // Assemble & Act
            var factory = CreateIntructionFactory();
            var add = factory.Get(1001) as Add;

            // Assert
            Assert.Equal(ParameterMode.Position, add.Param1Mode);
            Assert.Equal(ParameterMode.Immediate, add.Param2Mode);
        }

        [Fact]
        public void SetsCorrectParameterModes3()
        {
            // Assemble & Act
            var factory = CreateIntructionFactory();
            var output = factory.Get(104) as Output;

            // Assert
            Assert.Equal(ParameterMode.Immediate, output.Param1Mode);
        }

        [Fact]
        public void CreatesCorrectInstructionObjects_Test3()
        {
            // Assemble & Act
            var factory = CreateIntructionFactory();
            var add = factory.Get(1001);
            var multiply = factory.Get(1102);
            var input = factory.Get(103);
            var output = factory.Get(04);
            var halt = factory.Get(99);

            // Assert
            Assert.IsType<Add>(add);
            Assert.IsType<Multiply>(multiply);
            Assert.IsType<Input>(input);
            Assert.IsType<Output>(output);
            Assert.IsType<Halt>(halt);
        }
    }
}
