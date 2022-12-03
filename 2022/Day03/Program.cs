var input = File.ReadAllLines("input.txt");

// Part 1
int totalPriority = 0;
foreach (var rucksack in input)
{
    totalPriority += GetPriorityForRucksack(rucksack);
}

Console.WriteLine($"Total priority: {totalPriority}");

// Part 2
int totalBadgePriority = 0;
foreach (var rucksackGroup in BatchedEnumerable(input, 3))
{
    char badge = GetDuplicatedChar(rucksackGroup[0], rucksackGroup[1], rucksackGroup[2]);
    totalBadgePriority += GetPriorityForChar(badge);
}

Console.WriteLine($"Total badge priority: {totalBadgePriority}");

static int GetPriorityForRucksack(string rucksack)
{
    int compartmentSize = rucksack.Length / 2;
    string firstCompartment = rucksack[..compartmentSize];
    string secondCompartment = rucksack[compartmentSize..];
    char duplicate = GetDuplicatedChar(firstCompartment, secondCompartment);
    return GetPriorityForChar(duplicate);
}

static char GetDuplicatedChar(params string[] strings)
{
    if (strings.Length < 2)
        throw new InvalidOperationException("Must pass at least 2 strings for comparison");
    
    foreach (char ch in strings[0])
    {
        if (strings[1..].All(s => s.Contains(ch)))
            return ch;
    }

    throw new InvalidOperationException("No duplicated character across all strings");
}

static int GetPriorityForChar(char ch)
{
    return ch switch
    {
        >= 'a' and <= 'z' => ch - 96, // 1-26
        >= 'A' and <= 'Z' => ch - 38, // 27-52
        _ => throw new ArgumentOutOfRangeException(nameof(ch), ch, "Character must be between a-z or A-Z")
    };
}

static IEnumerable<string[]> BatchedEnumerable(IEnumerable<string> rucksacks, int batchSize)
{
    using var enumerator = rucksacks.GetEnumerator();

    while (enumerator.MoveNext())
    {
        var group = new string[batchSize];
        group[0] = enumerator.Current;
        for (int i = 1; i < batchSize; i++)
        {
            if (enumerator.MoveNext())
                group[i] = enumerator.Current;
        }

        yield return group;
    }
}