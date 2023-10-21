using FluentAssertions;

namespace Day22.Tests;

public class CubeFunctionTests
{
    [Fact]
    public void GivenLength2ThenFindEdgeLengthReturnsCorrectResult()
    {
        var mapString = """
                          ..
                          ..
                        ......
                        ......
                          ..
                          ..
                          ..
                          ..
                        """;

        var map = MapFunctions.CreateMapFromInputString(mapString);
        CubeFunctions.FindEdgeLength(map).Should().Be(2);
    }
    
    [Fact]
    public void GivenLength4ThenFindEdgeLengthReturnsCorrectResult()
    {
        var mapString = """
                                ...#
                                .###
                                ....
                                ....
                        .#....##....
                        .......#....
                        .#....##.###
                        .#..........
                                ..######
                                ..######
                                ..######
                                ..######
                        """;

        var map = MapFunctions.CreateMapFromInputString(mapString);
        CubeFunctions.FindEdgeLength(map).Should().Be(4);
    }

    [Fact]
    public void GetEdgesFromMapReturnsCorrectResult()
    {
        var mapString = """
                                ...#
                                .###
                                ....
                                ....
                        .#....##....
                        .......#....
                        .#....##.###
                        .#..........
                                ..######
                                ..######
                                ..######
                                ..######
                        """;

        var map = MapFunctions.CreateMapFromInputString(mapString);
        var edges = CubeFunctions.GetEdgesFromMap(map);

        var expected = new List<Edge>
        {
            new(new Position(8, 0), new Position(11, 0), Facing.Up),
            new(new Position(11,0), new Position(11, 3), Facing.Right),
            new(new Position(11, 4), new Position(11, 7), Facing.Right),
            new(new Position(12, 8), new Position(15, 8), Facing.Up),
            new(new Position(15, 8), new Position(15, 11), Facing.Right),
            new(new Position(15,11), new Position(12, 11), Facing.Down),
            new(new Position(11, 11), new Position(8, 11), Facing.Down),
            new(new Position(8, 11), new Position(8, 8), Facing.Left),
            new(new Position(7,7), new Position(4, 7), Facing.Down),
            new(new Position(3,7), new Position(0, 7), Facing.Down),
            new(new Position(0,7), new Position(0, 4), Facing.Left),
            new(new Position(0,4), new Position(3, 4), Facing.Up),
            new(new Position(4,4), new Position(7, 4), Facing.Up),
            new(new Position(8,3), new Position(8, 0), Facing.Left),
        };

        edges.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void GivenEdgesOfLength2ThenGetEdgeMapReturnsCorrectResult()
    {
        var mapString = """
                          ..
                          ..
                        ......
                        ......
                          ..
                          ..
                          ..
                          ..
                        """;

        var map = MapFunctions.CreateMapFromInputString(mapString);
        var edgeMap = CubeFunctions.GetEdgeMap(map);

        Location L(int x, int y, Facing facing) => Location.Create(x, y, facing);
        var expected = new Dictionary<Location, Location>
        {
            // Edges distance 1
            [L(3, 0, Facing.Right)] = L(5, 2, Facing.Down),
            [L(3, 1, Facing.Right)] = L(4, 2, Facing.Down),
            [L(4,2,Facing.Up)] = L(3, 1, Facing.Left),
            [L(5,2,Facing.Up)] = L(3,0, Facing.Left),
            
            [L(4,3, Facing.Down)] = L(3,4, Facing.Left),
            [L(5,3, Facing.Down)] = L(3,5, Facing.Left),
            [L(3,4, Facing.Right)] = L(4,3, Facing.Up),
            [L(3,5,Facing.Right)] = L(5,3, Facing.Up),
            
            [L(2,5, Facing.Left)] = L(0,3, Facing.Up),
            [L(2,4, Facing.Left)] = L(1,3, Facing.Up),
            [L(0, 3, Facing.Down)] = L(2,5, Facing.Right),
            [L(1,3, Facing.Down)] = L(2,4, Facing.Right),
            
            [L(0,2, Facing.Up)] = L(2,0, Facing.Right),
            [L(1, 2, Facing.Up)] = L(2, 1, Facing.Right),
            [L(2,0, Facing.Left)] = L(0,2, Facing.Down),
            [L(2, 1, Facing.Left)] = L(1, 2, Facing.Down),
            
            // Edges distance 2
            [L(5,2, Facing.Right)] = L(3, 7, Facing.Left),
            [L(5, 3, Facing.Right)] = L(3, 6, Facing.Left),
            [L(3,7, Facing.Right)] = L(5,2, Facing.Left),
            [L(3,6, Facing.Right)] = L(5,3, Facing.Left),
            
            [L(2,6, Facing.Left)] = L(0,3, Facing.Right),
            [L(2, 7, Facing.Left)] = L(0, 2, Facing.Right),
            [L(0, 3, Facing.Left)] = L(2,6,Facing.Right),
            [L(0,2, Facing.Left)] = L(2, 7, Facing.Right),
            
            // Edges distance 4
            [L(2,0, Facing.Up)] = L(2, 7, Facing.Up),
            [L(3,0, Facing.Up)] = L(3, 7, Facing.Up),
            [L(2,7, Facing.Down)] = L(2, 0, Facing.Down),
            [L(3,7, Facing.Down)] = L(3, 0, Facing.Down),
        };
        
        edgeMap.Should().BeEquivalentTo(expected);
    }
    
}