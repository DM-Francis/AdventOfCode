using Day09;

var input = File.ReadAllLines("input.txt");

// Part 1
var rope = new Rope();
MoveRopeAccordingToInput(rope, input);
int uniqueTailPositions = rope.TailPositionHistory.Count;
Console.WriteLine($"Unique tail positions: {uniqueTailPositions}");

// Part 2
var longRope = new Rope(10);
MoveRopeAccordingToInput(longRope, input);
int uniqueLongTailPositions = longRope.TailPositionHistory.Count;
Console.WriteLine($"Unique tail positions for long rope: {uniqueLongTailPositions}");

static void MoveRopeAccordingToInput(Rope rope, IEnumerable<string> input)
{
    foreach (var line in input)
    {
        var split = line.Split(" ");
        var direction = GetDirection(split[0]);
        int distance = int.Parse(split[1]);

        rope.MoveHead(direction, distance);
    }
}

static Direction GetDirection(string value)
{
    return value switch
    {
        "L" => Direction.Left,
        "R" => Direction.Right,
        "U" => Direction.Up,
        "D" => Direction.Down,
        _ => throw new ArgumentException("Unrecognised direction")
    };
}