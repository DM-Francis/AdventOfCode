var input = File.ReadAllLines("input.txt");
var exampleInput = """
                   ..............
                   ..............
                   .......#......
                   .....###.#....
                   ...#...#.#....
                   ....#...##....
                   ...#.###......
                   ...##.#.##....
                   ....#..#......
                   ..............
                   ..............
                   ..............
                   """.Split(Environment.NewLine);;


var grove = CreateGroveFromInput(input);

// Part 1
for (int i = 0; i < 10; i++)
{
    ProcessSingleRound(grove);
}

int emptyGroundCount = CountEmptyGround(grove);
Console.WriteLine($"Empty ground count: {emptyGroundCount}");

// Part 2
grove = CreateGroveFromInput(input);
int roundCount = 0;
int moveCount;
do
{
    moveCount = ProcessSingleRound(grove);
    roundCount++;
    if (roundCount % 100 == 0)
        Console.WriteLine($"Round {roundCount}");
} while (moveCount > 0);

Console.WriteLine($"First round with no moves: {roundCount}");

return;


static Grove CreateGroveFromInput(string[] input)
{
    var grove = new Grove();

    for (var y = 0; y < input.Length; y++)
    {
        var line = input[y];
        for (var x = 0; x < line.Length; x++)
        {
            var c = line[x];
            if (c == '#')
            {
                grove[new Position(x,y)] = new Elf(grove.ElfCount);
            }
        }
    }

    return grove;
}

static int ProcessSingleRound(Grove grove)
{
    var proposals = GenerateProposals(grove);
    int moveCount = ApplyMoves(grove, proposals);
    
    var directionToMove = grove.DirectionPriority[0];
    grove.DirectionPriority.RemoveAt(0);
    grove.DirectionPriority.Add(directionToMove);

    return moveCount;
}

static Dictionary<Position,List<Elf>> GenerateProposals(Grove grove)
{
    var proposals = new Dictionary<Position, List<Elf>>();
    foreach (var elfPosition in grove.Positions)
    {
        var adjacentElfPositions = GetAdjacentElves(elfPosition, grove);
        if (adjacentElfPositions.Count == 0)
            continue;

        foreach (var direction in grove.DirectionPriority)
        {
            var adjacents = GetAdjacentPositionsInDirection(elfPosition, direction);
            if (!adjacents.Any(x => adjacentElfPositions.Contains(x))) // There are no elfs in the given direction
            {
                var proposedPosition = adjacents[1];// The first index is directly north, south, east or west etc.
                if (proposals.TryGetValue(proposedPosition, out List<Elf>? elvesAtPosition))
                {
                    elvesAtPosition.Add(grove[elfPosition]);
                }
                else
                {
                    proposals[proposedPosition] = new List<Elf> { grove[elfPosition] };
                }
                break;
            }
        }
    }

    return proposals;
}

static List<Position> GetAdjacentElves(Position position, Grove grove)
{
    var adjacentPositions = GetAdjacentPositions(position);
    return adjacentPositions.Where(grove.HasElfAtPosition).ToList();
}

static List<Position> GetAdjacentPositions(Position position)
{
    var adjacentPositions = new List<Position>
    {
        position with { Y = position.Y - 1 }, // N
        new(X: position.X + 1, Y: position.Y - 1), // NE
        position with { X = position.X + 1 }, // E
        new (X: position.X + 1, Y: position.Y + 1), // SE
        position with { Y = position.Y + 1 }, // S
        new (X: position.X - 1, Y: position.Y + 1), // SW
        position with { X = position.X - 1 }, // W
        new (X: position.X - 1, Y: position.Y - 1), // NW
    };
    return adjacentPositions;
}

static List<Position> GetAdjacentPositionsInDirection(Position p, Direction direction)
{
    return direction switch
    {
        Direction.North => new List<Position> { new(p.X - 1, p.Y - 1), new(p.X, p.Y - 1), new(p.X + 1, p.Y - 1) },
        Direction.South => new List<Position> { new(p.X - 1, p.Y + 1), new(p.X, p.Y + 1), new(p.X + 1, p.Y + 1) },
        Direction.West => new List<Position> { new(p.X - 1, p.Y - 1), new(p.X - 1, p.Y), new(p.X - 1, p.Y + 1) },
        Direction.East => new List<Position> { new(p.X + 1, p.Y - 1), new(p.X + 1, p.Y), new(p.X + 1, p.Y + 1) },
        _ => throw new ArgumentOutOfRangeException(nameof(direction))
    };
}

static int ApplyMoves(Grove grove, Dictionary<Position, List<Elf>> proposals)
{
    int moveCount = 0;
    foreach (var position in proposals.Keys)
    {
        if (proposals[position].Count == 1)
        {
            var elf = proposals[position][0];
            grove[elf] = position;
            moveCount++;
        }
    }

    return moveCount;
}

static int CountEmptyGround(Grove grove)
{
    int minX = grove.Positions.Min(x => x.X);
    int maxX = grove.Positions.Max(x => x.X);
    int minY = grove.Positions.Min(x => x.Y);
    int maxY = grove.Positions.Max(x => x.Y);

    int emptyGround = 0;
    for (int y = minY; y <= maxY; y++)
    {
        for (int x = minX; x <= maxX; x++)
        {
            if (!grove.HasElfAtPosition(new Position(x,y)))
                emptyGround++;
        }
    }

    return emptyGround;
}


static void RenderGrove(Grove grove)
{
    int minX = grove.Positions.Min(x => x.X);
    int maxX = grove.Positions.Max(x => x.X);
    int minY = grove.Positions.Min(x => x.Y);
    int maxY = grove.Positions.Max(x => x.Y);

    for (int y = minY; y <= maxY; y++)
    {
        for (int x = minX; x <= maxX; x++)
        {
            if (grove.HasElfAtPosition(new Position(x,y)))
                Console.Write('#');
            else
                Console.Write('.');
        }
        
        Console.WriteLine();
    }
}

public record Elf(int Id);
public record Position(int X, int Y);

public class Grove
{
    private Dictionary<Position,Elf> _positionToElf = new();
    private Dictionary<Elf,Position> _elfToPosition = new();
    
    public int ElfCount => _elfToPosition.Count;
    public IEnumerable<Position> Positions => _positionToElf.Keys;
    public bool HasElfAtPosition(Position p) => _positionToElf.ContainsKey(p);

    public Position this[Elf elf]
    {
        get => _elfToPosition[elf];
        set
        {
            if (_elfToPosition.TryGetValue(elf, out var previousPosition))
                _positionToElf.Remove(previousPosition);
            
            _elfToPosition[elf] = value;
            _positionToElf[value] = elf;
        }
    }

    public Elf this[Position p]
    {
        get => _positionToElf[p];
        set
        {
            if (_positionToElf.TryGetValue(p, out var existingElf))
                _elfToPosition.Remove(existingElf);
            
            _positionToElf[p] = value;
            _elfToPosition[value] = p;
        }
    }
    
    public List<Direction> DirectionPriority { get; } = new()
    {
        Direction.North,
        Direction.South,
        Direction.West,
        Direction.East
    };
}

public enum Direction
{
    North,
    South,
    West,
    East,
}