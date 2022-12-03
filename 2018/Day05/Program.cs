var input = File.ReadAllLines("input.txt")[0];
var polymer = input.ToCharArray();

// Part 1
int finalCount = ReactPolymerAndGetCount(polymer);
Console.WriteLine($"Remaining unit count: {finalCount}");

// Part 2
var distinctTypes = Enumerable.Range(65, 26).Select(x => (char)x);

int minimumLength = polymer.Length;
char typeForMinimumLength = default;

foreach (char type in distinctTypes)
{
    int count = ReactPolymerAndGetCount(polymer, type);
    if (count < minimumLength)
    {
        minimumLength = count;
        typeForMinimumLength = type;
    }
}

Console.WriteLine($"Minimum possible polymer size: {minimumLength} by removing units of type {typeForMinimumLength}");



static int ReactPolymerAndGetCount(IEnumerable<char> polymer, char? typeToIgnore = null)
{
    var polymerReaction = new Stack<char>();

    foreach (char unit in polymer)
    {
        if (IsSameUnitType(unit, typeToIgnore.GetValueOrDefault()))
            continue;

        if (polymerReaction.Count == 0)
            polymerReaction.Push(unit);
        else if (TriggersReaction(polymerReaction.Peek(), unit))
            polymerReaction.Pop();
        else
            polymerReaction.Push(unit);
    }

    return polymerReaction.Count;
}

static bool TriggersReaction(char last, char current) => Math.Abs(last - current) == 32;

static bool IsSameUnitType(char first, char second)
{
    return first == second || TriggersReaction(first, second);
}