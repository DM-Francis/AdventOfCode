using Day18;
using System;
using Xunit;

namespace Day18.Test
{
    public class ExpressionEvaluatorTests
    {
        [Theory]
        [InlineData("1 + 2", 3)]
        [InlineData("4 * 2", 8)]
        [InlineData("10 * 2", 20)]
        public void SimpleExpressions(string expression, long expected)
        {
            // Act
            long result = Program.EvaluateExpression(expression);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("1 + 2 * 3", 9)]
        [InlineData("4 * 4 + 3 + 6 * 4", 100)]
        public void ExpressionsWithMultipleOperations(string expression, long expected)
        {
            // Act
            long result = Program.EvaluateExpression(expression);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("(2 + 4) * 8", 48)]
        [InlineData("6 * (4 + 5)", 54)]
        [InlineData("6 * (4 + 5) + (10 * 2)", 74)]
        [InlineData("(1 + 2 * 3) + 6 * (4 + 5)", 135)]
        public void ExpressionsWithBrackets(string expression, long expected)
        {
            // Act
            long result = Program.EvaluateExpression(expression);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("(2 + 4 * (10 + 12)) * 8", 1056)]
        [InlineData("6 * ((4 * 8) + 5)", 222)]
        [InlineData("6 * ((4 * 8) + 5) + (20 * 0)", 222)]
        public void ExpressionsWithNestedBrackets(string expression, long expected)
        {
            // Act
            long result = Program.EvaluateExpression(expression);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("2 * 3 + (4 * 5)", 26)]
        [InlineData("5 + (8 * 3 + 9 + 3 * 4 * 3)", 437)]
        [InlineData("5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))", 12240)]
        [InlineData("((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2", 13632)]
        public void ExamplesFromSite(string expression, long expected)
        {
            // Act
            long result = Program.EvaluateExpression(expression);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CanAddBracketsAroundAddition()
        {
            // Assemble
            string expression = "4 + 3";

            // Act
            string newExpression = Program.AddBracketsAroundAdditionOperations(expression);

            // Assert
            Assert.Equal("( 4 + 3 )", newExpression);
        }

        [Fact]
        public void CanAddBracketsAroundAddition2()
        {
            // Assemble
            string expression = "5 * 4 + 3 + 6";

            // Act
            string newExpression = Program.AddBracketsAroundAdditionOperations(expression);

            // Assert
            Assert.Equal("5 * ( 4 + 3 ) + 6", newExpression);
        }
    }
}
