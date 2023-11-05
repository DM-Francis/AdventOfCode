using Day24;

var input = File.ReadAllLines("input");
var exampleInput = """
                   sesenwnenenewseeswwswswwnenewsewsw
                   neeenesenwnwwswnenewnwwsewnenwseswesw
                   seswneswswsenwwnwse
                   nwnwneseeswswnenewneswwnewseswneseene
                   swweswneswnenwsewnwneneseenw
                   eesenwseswswnenwswnwnwsewwnwsene
                   sewnenenenesenwsewnenwwwse
                   wenwwweseeeweswwwnwwe
                   wsweesenenewnwwnwsenewsenwwsesesenwne
                   neeswseenwwswnwswswnw
                   nenwswwsewswnenenewsenwsenwnesesenew
                   enewnwewneswsewnwswenweswnenwsenwsw
                   sweneswneswneneenwnewenewwneswswnese
                   swwesenesewenwneswnwwneseswwne
                   enesenwswwswneneswsenwnewswseenwsese
                   wnwnesenesenenwwnenwsewesewsesesew
                   nenewswnwewswnenesenwnesewesw
                   eneswnwswnwsenenwnwnwwseeswneewsenese
                   neswnwewnwnwseenwseesewsenwsweewe
                   wseweeenwnesenwwwswnew
                   """.Split(Environment.NewLine);


var allInstructions = new List<List<Direction>>();
foreach (string line in input)
{
    var instructions = ParseInstructionLine(line);
    allInstructions.Add(instructions);
}

var grid = new HexGrid();

foreach (var instructionSet in allInstructions)
{
    var current = new TilePosition(0, 0, 0);
    foreach (var instruction in instructionSet)
    {
        current = current.GetNeighbourInDirection(instruction);
    }

    grid[current] = grid[current].Flip();
}


Console.WriteLine($"Black tile count: {grid.BlackTiles}");

// Part 2
for (int i = 0; i < 100; i++)
{
    AdvanceGrid1Day(grid);
}

Console.WriteLine($"Black tiles after 100 days: {grid.BlackTiles}");

return;

static List<Direction> ParseInstructionLine(string line)
{
    var instructions = new List<Direction>();
    using var enumerator = line.GetEnumerator();
    while (enumerator.MoveNext())
    {
        switch (enumerator.Current)
        {
            case 'e':
                instructions.Add(Direction.E);
                break;
            case 'w':
                instructions.Add(Direction.W);
                break;
            case 'n':
                enumerator.MoveNext();
                instructions.Add(enumerator.Current switch
                {
                    'e' => Direction.NE,
                    'w' => Direction.NW,
                    _ => throw new InvalidOperationException("Invalid character")
                });
                break;
            case 's':
                enumerator.MoveNext();
                instructions.Add(enumerator.Current switch
                {
                    'e' => Direction.SE,
                    'w' => Direction.SW,
                    _ => throw new InvalidOperationException("Invalid character")
                });
                break;
            default:
                throw new InvalidOperationException("Invalid character");
        }
    }

    return instructions;
}

static void AdvanceGrid1Day(HexGrid grid)
{
    var adjacentBlacks = grid.Hexes.Where(h => h.Value == Colour.Black)
        .ToDictionary(h => h.Key, _ => 0);

    foreach (var position in grid.AllTilePositions())
    {
        if (grid[position] != Colour.Black)
            continue;
        
        foreach (var neighbour in position.GetAllNeighbours())
        {
            int current = adjacentBlacks.GetValueOrDefault(neighbour);
            adjacentBlacks[neighbour] = current + 1;
        }
    }

    var toFlip = new Dictionary<TilePosition, Colour>();
    foreach (var (position, count) in adjacentBlacks)
    {
        if (grid[position] == Colour.Black && count is 0 or > 2)
            toFlip[position] = Colour.White;
        else if (grid[position] == Colour.White && count == 2)
            toFlip[position] = Colour.Black;
    }

    foreach (var (position, newColour) in toFlip)
    {
        grid[position] = newColour;
    }
}