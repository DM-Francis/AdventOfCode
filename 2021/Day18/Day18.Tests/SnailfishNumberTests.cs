using FluentAssertions;
using Xunit.Sdk;

namespace Day18.Tests;

public class SnailfishNumberTests
{
    [Fact]
    public void CanParseBasicSnailfishNumbers()
    {
        var input = "[1,2]";

        var result = SnailfishNumber.Parse(input);

        var expected = new SnailfishNumber
        {
            Root = new Pair
            {
                Left = new Number { Value = 1 },
                Right = new Number { Value = 2 }
            }
        };
        expected.Root.Left.Parent = expected.Root;
        expected.Root.Right.Parent = expected.Root;

        result.Should().BeEquivalentTo(expected, config =>
            config.IgnoringCyclicReferences());
        
        result.Root.Depth.Should().Be(0);
        result.Root.Left.Depth.Should().Be(1);
    }

    [Fact]
    public void CanParseMoreComplexSnailfishNumber()
    {
        var input = "[[1,2],3]";
        var result = SnailfishNumber.Parse(input);

        var expected = new SnailfishNumber
        {
            Root = new Pair
            {
                Left = new Pair
                {
                    Left = new Number { Value = 1 },
                    Right = new Number { Value = 2 }
                },
                Right = new Number { Value = 3 }
            }
        };
        expected.Root.UpdateParentProperties();

        result.Should().BeEquivalentTo(expected, config =>
            config.IgnoringCyclicReferences());
    }
    
    [Fact]
    public void CanParseVeryComplexSnailfishNumber()
    {
        var input = "[[8,[[8,6],[6,0]]],[8,[[1,8],1]]]";
        var result = SnailfishNumber.Parse(input);
     
        var expected = new SnailfishNumber
        {
            Root = new Pair
            {
                Left = new Pair
                {
                    Left = new Number { Value = 8 },
                    Right = new Pair
                    {
                        Left = new Pair
                        {
                            Left = new Number { Value = 8 },
                            Right = new Number { Value = 6 }
                        },
                        Right = new Pair
                        {
                            Left = new Number { Value = 6 },
                            Right = new Number { Value = 0 }
                        }
                    }
                },
                Right = new Pair
                {
                    Left = new Number { Value = 8 },
                    Right = new Pair
                    {
                        Left = new Pair
                        {
                            Left = new Number { Value = 1 },
                            Right = new Number { Value = 8 },
                        },
                        Right = new Number { Value = 1 }
                    }
                }
            }
        };
        
        expected.Root.UpdateParentProperties();
     
        result.Should().BeEquivalentTo(expected, config =>
            config.IgnoringCyclicReferences());
    }

    [Theory]
    [InlineData("[[[[[9,8],1],2],3],4]", "[[[[0,9],2],3],4]")]
    [InlineData("[7,[6,[5,[4,[3,2]]]]]", "[7,[6,[5,[7,0]]]]")]
    [InlineData("[[6,[5,[4,[3,2]]]],1]", "[[6,[5,[7,0]]],3]")]
    [InlineData("[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]", "[[3,[2,[8,0]]],[9,[5,[7,0]]]]")]
    [InlineData("[[3,[2,[1,[7,3]]]],[6,[5,[4,[3,2]]]]]", "[[3,[2,[8,0]]],[9,[5,[7,0]]]]")]
    public void CanDoSingleExplosionsCorrectly(string input, string expected)
    {
        var number = SnailfishNumber.Parse(input);
        number.Reduce();
        number.ToString().Should().Be(expected);
    }

    [Theory]
    [InlineData("[[[[[4,3],4],4],[7,[[8,4],9]]],[1,1]]", "[[[[0,7],4],[[7,8],[6,0]]],[8,1]]")]
    public void CanDoComplexReductions(string input, string expected)
    {
        var number = SnailfishNumber.Parse(input);
        number.Reduce();
        number.ToString().Should().Be(expected);
    }
}