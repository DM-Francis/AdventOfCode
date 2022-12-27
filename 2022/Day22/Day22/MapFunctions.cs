using System.Text;

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

    public static Location FollowCommandsOnMap(MapSquare[,] map, Location startingLocation,
        IEnumerable<object> commands)
    {
        var path = new List<Location> { startingLocation };
        var currentLocation = startingLocation;
        foreach (var command in commands)
        {
            // Console.WriteLine(command);
            currentLocation = command switch
            {
                'L' => currentLocation.TurnLeft(),
                'R' => currentLocation.TurnRight(),
                int distance => MoveForward(map, currentLocation, distance, path),
                _ => throw new ArgumentOutOfRangeException(nameof(command), command, "Unrecognised command")
            };
            path.Add(currentLocation);

            // RenderMapWithPath(map, path);
        }

        RenderMapWithPath(map, path);
        return currentLocation;
    }

    public static Location MoveForward(MapSquare[,] map, Location location, int distance, IList<Location>? path = null)
    {
        var direction = location.Facing;
        var position = location.Position;
        for (int i = 0; i < distance; i++)
        {
            var next = position.MoveInDirection(direction);

            if (next.X < 0)
                next = FindEdgeInDirection(map, position, Facing.Right);
            else if (next.Y < 0)
                next = FindEdgeInDirection(map, position, Facing.Down);
            else if (next.X > map.GetUpperBound(0))
                next = FindEdgeInDirection(map, position, Facing.Left);
            else if (next.Y > map.GetUpperBound(1))
                next = FindEdgeInDirection(map, position, Facing.Up);
            else if (map[next.X, next.Y] == MapSquare.Empty)
                next = FindEdgeInDirection(map, position, GetOppositeFacing(direction));

            if (map[next.X, next.Y] == MapSquare.Wall)
                return new Location(position, direction);

            position = next;
            path?.Add(new Location(position, direction));
        }

        return new Location(position, direction);
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

    public static void RenderMapWithPath(MapSquare[,] map, IReadOnlyList<Location> path)
    {
        Console.Clear();
        int origRow = Console.CursorTop;
        int origCol = Console.CursorLeft;
        for (int y = 0; y < map.GetLength(1); y++)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                var output = map[x, y] switch
                {
                    MapSquare.Empty => ' ',
                    MapSquare.Open => '.',
                    MapSquare.Wall => '#',
                    _ => throw new IndexOutOfRangeException()
                };

                RenderAt(output, x, y, ConsoleColor.White, origRow, origCol);
            }
        }

        var previous = path[0];
        foreach (var location in path)
        {
            int diff = Math.Abs(location.Position.X - previous.Position.X) +
                       Math.Abs(location.Position.Y - previous.Position.Y);

            if (diff % 50 != 49 && diff > 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write('X');
                break;
            }

            previous = location;

            var output = location.Facing switch
            {
                Facing.Right => '>',
                Facing.Down => 'v',
                Facing.Left => '<',
                Facing.Up => '^',
                _ => throw new IndexOutOfRangeException()
            };
            
            RenderAt(output, location.Position.X, location.Position.Y, ConsoleColor.Green, origRow, origCol);
            // Thread.Sleep(10);
        }
        
        Console.SetCursorPosition(0, origRow + map.GetLength(1));
    }

    private static void RenderAt(char c, int x, int y, ConsoleColor color, int origRow, int origCol)
    {
        Console.SetCursorPosition(origCol + x, origRow + y);
        Console.ResetColor();
        Console.ForegroundColor = color;
        Console.Write(c);
        Console.ResetColor();
    }
}