var input = File.ReadAllLines("input.txt");

int fullyContainedCount = 0;
int hasOverlapCount = 0;
foreach (string line in input)
{
    var split = line.Split(',');
    string firstAssignments = split[0];
    string secondAssignments = split[1];

    var range1 = GetRange(firstAssignments);
    var range2 = GetRange(secondAssignments);

    if (IsContainedInRange(range1, range2) || IsContainedInRange(range2, range1))
        fullyContainedCount++;

    if (HasAnyOverlap(range1, range2))
        hasOverlapCount++;
}

Console.WriteLine($"Count of pairs where one range contains the other: {fullyContainedCount}");
Console.WriteLine($"Count of pairs with any overlap between ranges {hasOverlapCount}");


static Range GetRange(string assignmentString)
{
    string[] split = assignmentString.Split('-');
    int start = int.Parse(split[0]);
    int end = int.Parse(split[1]);

    return new Range(start, end);
}

// Returns true if the the first range is contained completely by the second
static bool IsContainedInRange(Range first, Range second)
{
    return first.Start >= second.Start && first.End <= second.End;
}

static bool HasAnyOverlap(Range first, Range second)
{
    return first.Contains(second.Start) || first.Contains(second.End)
        || second.Contains(first.Start) || second.Contains(first.End);
}

public record Range(int Start, int End)
{
    public bool Contains(int value) => value >= Start && value <= End;
}