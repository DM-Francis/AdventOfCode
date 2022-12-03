PartOne();
PartTwo();

static void PartOne()
{
    var data = File.ReadAllLines("input.txt");

    int horizontal = 0;
    int depth = 0;

    foreach(string line in data)
    {
        (horizontal, depth) = UpdatePosition(horizontal, depth, line);
    }

    Console.WriteLine($"Final horizontal value: {horizontal}");
    Console.WriteLine($"Final depth value: {depth}");
    Console.WriteLine($"Multiplication result: {horizontal * depth}");
}

static (int Horizontal, int Depth) UpdatePosition(int horizontal, int depth, string input)
{
    var inputParts = input.Split(' ');
    string command = inputParts[0];
    int distance = int.Parse(inputParts[1]);

    return command switch
    {
        "forward" => (horizontal + distance, depth),
        "down" => (horizontal, depth + distance),
        "up" => (horizontal, depth - distance),
        _ => throw new ArgumentException("Invalid input", nameof(input)),
    };
}

static void PartTwo()
{
    var data = File.ReadAllLines("input.txt");

    var position = new SubmarinePosition(0, 0, 0);

    foreach (string line in data)
    {
        position = UpdateSubmarinePosition(position, line);
    }

    Console.WriteLine($"Final horizontal value: {position.Horizontal}");
    Console.WriteLine($"Final depth value: {position.Depth}");
    Console.WriteLine($"Multiplication result: {position.Horizontal * position.Depth}");
}

static SubmarinePosition UpdateSubmarinePosition(SubmarinePosition position, string input)
{
    var inputParts = input.Split(' ');
    string command = inputParts[0];
    int distance = int.Parse(inputParts[1]);

    int horizontal = position.Horizontal;
    int depth = position.Depth;
    int aim = position.Aim;

    return command switch
    {
        "forward" => position with { Horizontal = horizontal + distance, Depth = depth + distance * aim },
        "down" => position with { Aim = aim + distance },
        "up" => position with { Aim = aim - distance },
        _ => throw new ArgumentException("Invalid input", nameof(input)),
    };
}

record SubmarinePosition(int Horizontal, int Depth, int Aim);