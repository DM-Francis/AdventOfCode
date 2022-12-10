using Day10;

var input = File.ReadAllLines("input.txt");

var operations = GetOperationsFromInput(input);
var cpu = new Cpu();
foreach (var operation in operations)
{
    cpu.ApplyOperation(operation);
}

// Part 1
int signalStrengthSum = cpu.GetSignalStrengthDuringCycle(20) +
                       cpu.GetSignalStrengthDuringCycle(60) +
                       cpu.GetSignalStrengthDuringCycle(100) +
                       cpu.GetSignalStrengthDuringCycle(140) +
                       cpu.GetSignalStrengthDuringCycle(180) +
                       cpu.GetSignalStrengthDuringCycle(220);

Console.WriteLine($"Sum of signal strengths: {signalStrengthSum}");

// Part 2
RenderScreenFromRegisterValues(cpu.RegisterHistory); // PHLHJGZA


static IEnumerable<IOperation> GetOperationsFromInput(IEnumerable<string> input)
{
    foreach (var line in input)
    {
        var split = line.Split(" ");
        IOperation operation = split[0] switch
        {
            "noop" => new Noop(),
            "addx" => new AddX(int.Parse(split[1])),
            _ => throw new ArgumentException("Unrecognised operation")
        };

        yield return operation;
    }
}

static void RenderScreenFromRegisterValues(IReadOnlyList<int> registerValues)
{
    for (int cycle = 0; cycle < registerValues.Count; cycle++)
    {
        int x = registerValues[cycle];
        int pixel = cycle % 40;
        int[] spriteValues = { x - 1, x, x + 1 };

        Console.Write(spriteValues.Contains(pixel) ? '#' : '.');

        if (pixel == 39)
            Console.WriteLine();
    }
}