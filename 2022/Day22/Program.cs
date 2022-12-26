using System.Text;

var input = File.ReadAllText("input.txt");
var exampleInput = File.ReadAllText("example-input.txt");

var inputSplit = input.Split("\n\n");
var mapString = inputSplit[0];
var directionsString = inputSplit[1].TrimEnd('\n');

var map = CreateMapFromInputString(mapString);
var startingPosition = GetStartingSquare(map);
var commands = SplitIntoCommands(directionsString);

var lastLocation = FollowCommandsOnMap(map, new Location(startingPosition, Facing.Right), commands);
var finalPassword = (lastLocation.Position.Y + 1) * 1000 + (lastLocation.Position.X + 1) * 4 + (int)lastLocation.Facing;

Console.WriteLine($"Final password: {finalPassword}");
// Gets the wrong answer for the actual input - must be a subtle bug somewhere

static MapSquare[,] CreateMapFromInputString(string mapString)
{
    var mapLines = mapString.Split('\n');
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

static Position GetStartingSquare(MapSquare[,] map)
{
    for (int x = 0; x < map.GetLength(0); x++)
    {
        if (map[x, 0] != MapSquare.Empty)
            return new Position(x, 0);
    }

    throw new InvalidOperationException("Starting position not found on first row");
}

static List<object> SplitIntoCommands(string directionsString)
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

    return output;
}

static Location FollowCommandsOnMap(MapSquare[,] map, Location startingLocation, IEnumerable<object> commands)
{
    var path = new Dictionary<Position, Facing> { [startingLocation.Position] = startingLocation.Facing };
    var currentLocation = startingLocation;
    foreach (var command in commands)
    {
        // Console.WriteLine(command);
        currentLocation = command switch
        {
            'L' => currentLocation.TurnLeft(),
            'R' => currentLocation.TurnRight(),
            int distance => MoveForward(map, currentLocation, distance),
            _ => throw new ArgumentOutOfRangeException(nameof(command), command, "Unrecognised command")
        };
        path[currentLocation.Position] = currentLocation.Facing;
        
        // RenderMapWithPath(map, path);
    }

    RenderMapWithPath(map, path);
    return currentLocation;
}

static Location MoveForward(MapSquare[,] map, Location location, int distance)
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
    }

    return new Location(position, direction);
}

static Position FindEdgeInDirection(MapSquare[,] map, Position position, Facing direction)
{
    var current = position;
    var next = current;
    bool isInMap = true;

    while (isInMap && map[next.X, next.Y] != MapSquare.Empty)
    {
        current = next;
        next = current.MoveInDirection(direction);
        isInMap = next.X >= 0 && next.X <= map.GetUpperBound(0)&&
                  next.Y >= 0 && next.Y <= map.GetUpperBound(1);
    }

    return current;
}

static Facing GetOppositeFacing(Facing facing)
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

static void RenderMapWithPath(MapSquare[,] map, IReadOnlyDictionary<Position, Facing> path)
{
    Console.Clear();
    for (int y = 0; y < map.GetLength(1); y++)
    {
        for (int x = 0; x < map.GetLength(0); x++)
        {
            Console.ResetColor();
            var position = new Position(x, y);
            char output;
            if (path.TryGetValue(position, out var direction))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                output = direction switch
                {
                    Facing.Right => '>',
                    Facing.Down => 'v',
                    Facing.Left => '<',
                    Facing.Up => '^',
                    _ => throw new IndexOutOfRangeException()
                };
            }
            else
            {
                output = map[x, y] switch
                {
                    MapSquare.Empty => ' ',
                    MapSquare.Open => '.',
                    MapSquare.Wall => '#',
                    _ => throw new IndexOutOfRangeException()
                };
            }
            
            Console.Write(output);
        }
        
        Console.WriteLine();
    }
}

public record struct Position(int X, int Y)
{
    public Position MoveInDirection(Facing direction)
    {
        return direction switch
        {
            Facing.Right => this with { X = this.X + 1 },
            Facing.Down => this with { Y = this.Y + 1 },
            Facing.Left => this with { X = this.X - 1 },
            Facing.Up => this with { Y = this.Y - 1 },
            _ => throw new ArgumentOutOfRangeException(nameof(direction))
        };
    }
}

public record struct Location(Position Position, Facing Facing)
{
    public Location TurnLeft()
    {
        var newFacing = Facing switch
        {
            Facing.Right => Facing.Up,
            Facing.Down => Facing.Right,
            Facing.Left => Facing.Down,
            Facing.Up => Facing.Left,
            _ => throw new ArgumentOutOfRangeException(nameof(Facing))
        };

        return this with { Facing = newFacing };
    }
    
    public Location TurnRight()
    {
        var newFacing = Facing switch
        {
            Facing.Right => Facing.Down,
            Facing.Down => Facing.Left,
            Facing.Left => Facing.Up,
            Facing.Up => Facing.Right,
            _ => throw new ArgumentOutOfRangeException(nameof(Facing))
        };

        return this with { Facing = newFacing };
    }
}

public enum MapSquare
{
    Empty,
    Open,
    Wall
}

public enum Facing
{
    Right = 0,
    Down = 1,
    Left = 2,
    Up = 3
}