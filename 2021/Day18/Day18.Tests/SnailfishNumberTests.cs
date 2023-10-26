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
        expected.Root.SetParentProperties();

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
        
        expected.Root.SetParentProperties();
     
        result.Should().BeEquivalentTo(expected, config =>
            config.IgnoringCyclicReferences());
    }
}