using Day22.MapTypes;
using FluentAssertions;

namespace Day22.Tests;

public class MapFunctionTests
{
    [Theory]
    [InlineData(Facing.Right, Facing.Left)]
    [InlineData(Facing.Left, Facing.Right)]
    [InlineData(Facing.Up, Facing.Down)]
    [InlineData(Facing.Down, Facing.Up)]
    public void GetOppositeFacingReturnsCorrectValues(Facing input, Facing expected)
    {
        MapFunctions.GetOppositeFacing(input).Should().Be(expected);
    }
}