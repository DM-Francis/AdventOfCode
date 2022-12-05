using System.Runtime.CompilerServices;

var input = File.ReadAllLines("input.txt");

var startingStackDiagram = input[..8];
var moveCommands = input[10..];

var stacks = GetStacksFromDiagram(startingStackDiagram);

// Part 1
foreach (var command in moveCommands)
{
	var commandSplit = command.Split(' ');
	int count = int.Parse(commandSplit[1]);
	int from = int.Parse(commandSplit[3]);
	int to = int.Parse(commandSplit[5]);

	for (int i = 0; i < count; i++)
	{
		char box = stacks[from].Pop();
		stacks[to].Push(box);
	}
}

var boxesAtTopOfStacks = stacks[1..].Select(s => s.Peek()).ToArray();
Console.WriteLine($"Top boxes: {new string(boxesAtTopOfStacks)}");

// Part 2
stacks = GetStacksFromDiagram(startingStackDiagram);
foreach (var command in moveCommands)
{
    var commandSplit = command.Split(' ');
    int count = int.Parse(commandSplit[1]);
    int from = int.Parse(commandSplit[3]);
    int to = int.Parse(commandSplit[5]);

    var tempStack = new Stack<char>();
    for (int i = 0; i < count; i++)
    {
        tempStack.Push(stacks[from].Pop());
    }

    for (int i = 0; i < count; i++)
    {
        stacks[to].Push(tempStack.Pop());
    }
}

var boxesAtTopOfStacks2 = stacks[1..].Select(s => s.Peek()).ToArray();
Console.WriteLine($"Top boxes: {new string(boxesAtTopOfStacks2)}");


static Stack<char>[] GetStacksFromDiagram(string[] startingDiagram)
{
    var stacks = new Stack<char>[10];
    for (int i = 1; i < 10; i++) // Keep the 0 index empty so that the indexes match the input
    {
        stacks[i] = new Stack<char>();
    }

    foreach (var line in startingDiagram.Reverse())
    {
        int stackIndex = 1;
        for (int charIndex = 1; charIndex < 34; charIndex += 4)
        {
            char box = line[charIndex];
            if (box != ' ')
                stacks[stackIndex].Push(box);

            stackIndex++;
        }
    }

    return stacks;
}