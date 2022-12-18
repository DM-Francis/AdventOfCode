using Day17;

var wind = File.ReadAllText("input.txt").Trim('\n');
// var wind = ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>";
var shapesInput = File.ReadAllText("shapes.txt");
var individualShapeStrings = shapesInput.Split("\r\n\r\n");

var shapes = individualShapeStrings.Select(Shape.FromString).ToList();

int windIndex = 0;
var chamber = new SortedSet<Position>(); // All coordinates where there is a (stationary) piece of rock

var heightOffsets = new Dictionary<long, int>();
int baseHeight = 0;
int repetitionEndHeight = 0;

bool hasRepeated = false;

int rockNumber = 0;
int repetitionPeriodStart = 0;
int repetitionPeriodEnd = 0;

while (!hasRepeated)
{
    switch (windIndex / wind.Length)
    {
        case 1 when repetitionPeriodStart == 0:
            repetitionPeriodStart = rockNumber;
            baseHeight = GetTowerHeight(chamber);
            heightOffsets[0] = 0;
            break;
        case 1:
            heightOffsets[rockNumber - repetitionPeriodStart] = GetTowerHeight(chamber) - baseHeight;
            break;
        case 2:
            hasRepeated = true;
            repetitionPeriodEnd = rockNumber;
            repetitionEndHeight = GetTowerHeight(chamber) - baseHeight;
            break;
    }

    var currentRock = GenerateNextRock(rockNumber, chamber, shapes);
    bool isAtRest = false;

    var startingPos = currentRock.Position;
    
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

    var finishingPos = currentRock.Position;
    int currentHeight = chamber.Max(p => p.Y);
    Console.WriteLine($"Simulated rock number: {rockNumber + 1}.  Movement: ({finishingPos.X - startingPos.X}, {finishingPos.Y - startingPos.Y}).  Height: {currentHeight} Wind Index: {windIndex % wind.Length} Shape Index: {rockNumber % shapes.Count}");

    rockNumber++;
}


int heightOfTower = chamber.Max(p => p.Y);
Console.WriteLine($"Height of tower of 2022 rocks: {heightOfTower}");

// Part 2
/*
Investigate the results for repetitions.
Found an interesting repetition once we get to the end of the wind inputs.

Let n = rock number

At n = 3399, Height = 5370 (+0 from the previous rock)

The pattern repeats at n = 5094, Height = 8041 (again +0 from the previous rock)
It repeats again at n = 6789, Height = 10712

So, after a certain point, the pattern repeats every 1,695 rocks and the height increases by 2,671 each cycle

If N = 3399 + k * 1695, then the height after N rocks, H(N) = 5370 + k * 2671

For numbers between these 'multiples' of 1695 (e.g. with remainder r), we need to work out the height offset.
So for any int N, let

N = 3399 + k * 1695 + r, then the height after N rocks, H(N) = 5370 + k * 2671 + h(r)

where h(r) is the height offset for the remainder.

To find k and r:
k = (N - 3399) / 1695 using integer division
r = (N - 3399) % 1695

Then to find H:

H(N) = 5370 + k * 2671 + h(r)
 */
long period = repetitionPeriodEnd - repetitionPeriodStart;


long N = 1000000000000;
long k = (N - repetitionPeriodStart) / period;
long r = (N - repetitionPeriodStart) % period;

long height = baseHeight + k * repetitionEndHeight + heightOffsets[r];

Console.WriteLine($"Height after {N} rocks: {height}");


static Rock GenerateNextRock(int currentRockNumber, IEnumerable<Position> chamber, IReadOnlyList<Shape> shapes)
{
    var highestRockY = chamber.DefaultIfEmpty().Max(p => p.Y);
    const int rockStartingX = 3;
    return new Rock(new Position(rockStartingX, highestRockY + 4), shapes[currentRockNumber % shapes.Count]);
}

static Rock ApplyWindToRock(Rock rock, IEnumerable<Position> chamber, char windDirection)
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

static Rock ApplyFallToRock(Rock rock, IEnumerable<Position> chamber, out bool comeToRest)
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

static int GetTowerHeight(IEnumerable<Position> chamber)
{
    return chamber.DefaultIfEmpty().Max(p => p.Y);
}

static void RenderTopOfChamberWithRock(IReadOnlySet<Position> chamber, Rock rock)
{
    const int wallRight = 8;
    int maxY = Math.Max(rock.Position.Y + rock.Shape.Height - 1, GetTowerHeight(chamber));
    int minY = Math.Max(maxY - 40, 0);

    Console.Clear();
    for (int y = maxY; y >= minY; y--)
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
                    if (chamber.Contains(new Position(x,y)))
                        Console.Write('#');
                    else if (rock.OverlapsWithPosition(new Position(x,y)))
                        Console.Write('@');
                    else
                        Console.Write('.');
                    break;
            }
        }
        Console.WriteLine();
    }
    
}

public readonly record struct Position(int X, int Y) : IComparable<Position>
{
    public int CompareTo(Position other)
    {
        if (Y == other.Y)
            return X.CompareTo(other.X);

        return -Y.CompareTo(other.Y);
    }
}

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
        return _positions.Contains(other);
    }
}
