using Day17;

var wind = File.ReadAllText("input.txt").Trim('\n');
var shapesInput = File.ReadAllText("shapes.txt");
var individualShapeStrings = shapesInput.Split("\r\n\r\n");

var shapes = individualShapeStrings.Select(Shape.FromString).ToList();

int windIndex = 0;
var chamber = new HashSet<Position>(); // All coordinates where there is a (stationary) piece of rock

for (int rockNumber = 0; rockNumber < 2022; rockNumber++)
{
    Console.WriteLine($"Simulating rock number: {rockNumber}");
    var currentRock = GenerateNextRock(rockNumber, chamber, shapes);
    bool isAtRest = false;

    while (!isAtRest)
    {
        currentRock = ApplyWindToRock(currentRock, chamber, wind[windIndex++ % wind.Length]);
        // RenderChamberWithRock(chamber, currentRock);
        currentRock = ApplyFallToRock(currentRock, chamber, out isAtRest);
        // RenderChamberWithRock(chamber, currentRock);
    }

    foreach (var position in currentRock.AllPositions)
    {
        chamber.Add(position);
    }
    
    // RenderChamberWithRock(chamber, currentRock);
}

int heightOfTower = chamber.Max(p => p.Y);
Console.WriteLine($"Height of tower of 2022 rocks: {heightOfTower}");

static Rock GenerateNextRock(int currentRockNumber, HashSet<Position> chamber, IReadOnlyList<Shape> shapes)
{
    var highestRockY = chamber.DefaultIfEmpty().Max(p => p.Y);
    const int rockStartingX = 3;
    return new Rock(new Position(rockStartingX, highestRockY + 4), shapes[currentRockNumber % shapes.Count]);
}

static Rock ApplyWindToRock(Rock rock, HashSet<Position> chamber, char windDirection)
{
    int xChange = windDirection switch
    {
        '>' => 1,
        '<' => -1,
        _ => throw new ArgumentOutOfRangeException(nameof(windDirection), windDirection, "Invalid wind character")
    };
    var newRockPosition = new Rock(rock.Position with { X = rock.Position.X + xChange }, rock.Shape);
    if (newRockPosition.AllPositions.Any(p => p.X is 0 or 8) || // Hit either wall
        chamber.Any(p => newRockPosition.OverlapsWithPosition(p))) // Hit stopped rock
        return rock;

    return newRockPosition;
}

static Rock ApplyFallToRock(Rock rock, HashSet<Position> chamber, out bool comeToRest)
{
    var movedDownRock = new Rock(rock.Position with { Y = rock.Position.Y - 1 }, rock.Shape);

    if (chamber.Any(p => movedDownRock.OverlapsWithPosition(p)) ||
        movedDownRock.AllPositions.Any(p => p.Y == 0))
    {
        comeToRest = true;
        return rock;
    }

    comeToRest = false;
    return movedDownRock;
}

static void RenderChamberWithRock(HashSet<Position> chamber, Rock rock)
{
    const int wallRight = 8;
    int maxY = Math.Max(rock.Position.Y + rock.Shape.Height - 1, chamber.DefaultIfEmpty().Max(p => p.Y));

    Console.Clear();
    for (int y = maxY; y >= 0; y--)
    {
        for (int x = 0; x <= wallRight; x++)
        {
            switch (x, y)
            {
                case (0 or 8,0):
                    Console.Write('+');
                    break;
                case (_, 0):
                    Console.Write('-');
                    break;
                case (0 or 8, _):
                    Console.Write('|');
                    break;
                default:
                    if (rock.OverlapsWithPosition(new Position(x,y)))
                        Console.Write('@');
                    else if (chamber.Contains(new Position(x,y)))
                        Console.Write('#');
                    else
                        Console.Write('.');
                    break;
            }
        }
        Console.WriteLine();
    }
    
}

public record struct Position(int X, int Y);

public class Rock
{
    public Position Position { get; }
    public Shape Shape { get; }

    private readonly List<Position> _positions;
    public IEnumerable<Position> AllPositions => _positions.AsReadOnly();

    public Rock(Position position, Shape shape)
    {
        Position = position;
        Shape = shape;
        _positions = GeneratePositions(position, shape).ToList();
    }

    private static IEnumerable<Position> GeneratePositions(Position position, Shape shape)
    {
        for (int y = 0; y < shape.Height; y++)
        {
            for (int x = 0; x < shape.Width; x++)
            {
                if (shape[x, y])
                    yield return new Position(position.X + x, position.Y + y);
            }
        }
    }

    public bool OverlapsWithPosition(Position other)
    {
        return _positions.Any(p => p == other);
    }
}
