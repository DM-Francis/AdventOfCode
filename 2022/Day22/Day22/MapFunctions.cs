using System.Text;
using Day22.Rendering;

namespace Day22;

public static class MapFunctions
{
    public static MapSquare[,] CreateMapFromInputString(string mapString)
    {
        var mapLines = mapString.Split(new []{'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries);
        var map = new MapSquare[mapLines.Max(l => l.Length), mapLines.Length];

        for (int y = 0; y < map.GetLength(1); y++)
        {
            var line = mapLines[y];
            for (int x = 0; x < map.GetLength(0); x++)
            {
                if (x >= line.Length)
                    break;

                map[x, y] = line[x] switch
                {
                    ' ' => MapSquare.Empty,
                    '.' => MapSquare.Open,
                    '#' => MapSquare.Wall,
                    _ => throw new ArgumentOutOfRangeException(nameof(line), line[x], "Unrecognised map character")
                };
            }
        }

        return map;
    }

    public static Position GetStartingSquare(MapSquare[,] map)
    {
        for (int x = 0; x < map.GetLength(0); x++)
        {
            if (map[x, 0] != MapSquare.Empty)
                return new Position(x, 0);
        }

        throw new InvalidOperationException("Starting position not found on first row");
    }

    public static List<object> SplitIntoCommands(string directionsString)
    {
        var output = new List<object>();
        var numberBuilder = new StringBuilder();
        foreach (var ch in directionsString)
        {
            if (int.TryParse(ch.ToString(), out int _))
            {
                numberBuilder.Append(ch);
            }
            else
            {
                if (numberBuilder.Length > 0)
                {
                    int number = int.Parse(numberBuilder.ToString());
                    output.Add(number);
                    numberBuilder.Clear();
                }

                output.Add(ch);
            }
        }
        
        if (numberBuilder.Length > 0)
        {
            int number = int.Parse(numberBuilder.ToString());
            output.Add(number);
            numberBuilder.Clear();
        }

        return output;
    }
    
    public delegate void UpdatePathCallback(IReadOnlyList<Location> path);

    public static Location FollowCommandsOnMap(MapSquare[,] map, Location startingLocation,
        IEnumerable<object> commands, bool useCubeEdges = false, TimeSpan delay = default, UpdatePathCallback? callback = null)
    {
        var path = new List<Location> { startingLocation };
        var currentLocation = startingLocation;

        var cubeEdgeMap = CubeFunctions.GetEdgeMap(map);
        
        foreach (var command in commands)
        {
            currentLocation = command switch
            {
                'L' => currentLocation.TurnLeft(),
                'R' => currentLocation.TurnRight(),
                int distance => MoveForward(map, currentLocation, distance, path, useCubeEdges, cubeEdgeMap),
                _ => throw new ArgumentOutOfRangeException(nameof(command), command, "Unrecognised command")
            };
            
            path.Add(currentLocation);
            callback?.Invoke(path);
            Thread.Sleep(delay);
        }

        callback?.Invoke(path);
        return currentLocation;
    }

    public static Location MoveForward(MapSquare[,] map,
        Location start,
        int distance,
        IList<Location>? path = null,
        bool useCubeEdges = false,
        Dictionary<Location, Location>? cubeEdgeMap = null)
    {
        var current = start;
        for (int i = 0; i < distance; i++)
        {
            var newPosition = current.Position.MoveInDirection(current.Facing);

            Location newLocation;
            if (useCubeEdges)
                newLocation = AccountForEdgeCollisionsOnCube(map, newPosition, current.Position, current.Facing, cubeEdgeMap);
            else
                newLocation = AccountForEdgeCollisions(map, newPosition, current.Position, current.Facing);

            if (map[newLocation.Position.X, newLocation.Position.Y] == MapSquare.Wall)
                return current;

            current = newLocation;
            path?.Add(current);
        }

        return current;
    }

    private static Location AccountForEdgeCollisionsOnCube(MapSquare[,] map, Position finish, Position start,
        Facing direction, Dictionary<Location, Location> edgeMap)
    {
        if (edgeMap.TryGetValue(new Location(start, direction), out var mappedLocation))
            return mappedLocation;

        return new Location(finish, direction);
    }

    private static Location AccountForEdgeCollisions(MapSquare[,] map, Position finish, Position start, Facing direction)
    {
        if (finish.X < 0)
            return new Location(FindEdgeInDirection(map, start, Facing.Right), direction);
        if (finish.Y < 0)
            return new Location(FindEdgeInDirection(map, start, Facing.Down), direction);
        if (finish.X > map.GetUpperBound(0))
            return new Location(FindEdgeInDirection(map, start, Facing.Left), direction);
        if (finish.Y > map.GetUpperBound(1))
            return new Location(FindEdgeInDirection(map, start, Facing.Up), direction);
        if (map[finish.X, finish.Y] == MapSquare.Empty)
            return new Location(FindEdgeInDirection(map, start, GetOppositeFacing(direction)), direction);
        
        return new Location(finish, direction);
    }

    public static Position FindEdgeInDirection(MapSquare[,] map, Position position, Facing direction)
    {
        var current = position;
        var next = current;
        bool isInMap = true;

        while (isInMap && map[next.X, next.Y] != MapSquare.Empty)
        {
            current = next;
            next = current.MoveInDirection(direction);
            isInMap = next.X >= 0 && next.X <= map.GetUpperBound(0) &&
                      next.Y >= 0 && next.Y <= map.GetUpperBound(1);
        }

        return current;
    }

    public static Facing GetOppositeFacing(Facing facing)
    {
        return facing switch
        {
            Facing.Left => Facing.Right,
            Facing.Up => Facing.Down,
            Facing.Right => Facing.Left,
            Facing.Down => Facing.Up,
            _ => throw new ArgumentOutOfRangeException(nameof(facing))
        };
    }
}