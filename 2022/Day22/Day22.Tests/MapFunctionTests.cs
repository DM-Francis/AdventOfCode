using System.Collections;
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

    public static IEnumerable<object[]> GetHardEdgeTestData()
    {
        yield return new object[] { new Position(2, 0), Facing.Left, new Position(0, 0) };
        yield return new object[] { new Position(0, 0), Facing.Right, new Position(2, 0) };
        yield return new object[] { new Position(1, 2), Facing.Up, new Position(1, 0) };
        yield return new object[] { new Position(1, 0), Facing.Down, new Position(1, 2) };
    }

    public static IEnumerable<object[]> GetSoftEdgeTestData()
    {
        yield return new object[] { new Position(3, 2), Facing.Left, new Position(1, 2) };
        yield return new object[] { new Position(1, 2), Facing.Right, new Position(3, 2) };
        yield return new object[] { new Position(2, 3), Facing.Up, new Position(2, 1) };
        yield return new object[] { new Position(2, 1), Facing.Down, new Position(2, 3) };
    }
    
    [Theory]
    [MemberData(nameof(GetHardEdgeTestData))]
    public void FindEdgeInDirection_CanFindHardEdges(Position startingPosition, Facing direction, Position edgePosition)
    {
        // 3x3 all open
        var mapString = """
                        ...
                        ...
                        ...
                        """;
        var map = MapFunctions.CreateMapFromInputString(mapString);
        var result = MapFunctions.FindEdgeInDirection(map, startingPosition, direction);
        result.Should().Be(edgePosition);
    }

    [Theory]
    [MemberData(nameof(GetSoftEdgeTestData))]
    public void FindEdgeInDirection_CanFindSoftEdges(Position startingPosition, Facing direction, Position edgePosition)
    {
        // 5x5, middle 3x3 are open
        var mapString = """
                             
                         ... 
                         ... 
                         ... 
                             
                        """;
        var map = MapFunctions.CreateMapFromInputString(mapString);
        var result = MapFunctions.FindEdgeInDirection(map, startingPosition, direction);
        result.Should().Be(edgePosition);
    }

    [Fact]
    public void MoveForward_MovesTheCorrectAmountGivenNoWalls()
    {
        var mapString = "..........";
        var map = MapFunctions.CreateMapFromInputString(mapString);
        var startingPosition = new Position(1, 0);
        var location = new Location(startingPosition, Facing.Right);

        var result = MapFunctions.MoveForward(map, location, 3);
        var expected = new Location(new Position(4, 0), Facing.Right);
        result.Should().Be(expected);
    }

    [Fact]
    public void MoveForward_StopsWhenHitsWall()
    {
        var mapString = "....#.....";
        var map = MapFunctions.CreateMapFromInputString(mapString);
        var startingPosition = new Position(1, 0);
        var location = new Location(startingPosition, Facing.Right);

        var result = MapFunctions.MoveForward(map, location, 6);
        var expected = new Location(new Position(3, 0), Facing.Right);
        result.Should().Be(expected);
    }
    
    [Fact]
    public void MoveForward_WrapsAroundWhenMeetsEdge()
    {
        var mapString = "....#.....";
        var map = MapFunctions.CreateMapFromInputString(mapString);
        var startingPosition = new Position(5, 0);
        var location = new Location(startingPosition, Facing.Right);

        var result = MapFunctions.MoveForward(map, location, 6);
        var expected = new Location(new Position(1, 0), Facing.Right);
        result.Should().Be(expected);
    }
    
    [Fact]
    public void MoveForward_StopsIfWallIsOnOppositeEdge()
    {
        var mapString = "#...#.....";
        var map = MapFunctions.CreateMapFromInputString(mapString);
        var startingPosition = new Position(9, 0);
        var location = new Location(startingPosition, Facing.Right);

        var result = MapFunctions.MoveForward(map, location, 6);
        var expected = new Location(new Position(9, 0), Facing.Right);
        result.Should().Be(expected);
    }
}