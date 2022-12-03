using System.Diagnostics.CodeAnalysis;

var input = File.ReadAllLines("input.txt");

// Part 1
int twoCount = 0;
int threeCount = 0;

foreach (var id in input)
{
    var charCounts = id.GroupBy(ch => ch).Select(g => g.Count()).ToList();
    if (charCounts.Contains(2))
        twoCount++;
    if (charCounts.Contains(3))
        threeCount++;
}

int checksum = twoCount * threeCount;

Console.WriteLine($"Checksum: {checksum}");

// Part 2
string commonLetters = GetCommonLettersBetweenCorrectBoxIds(input);

Console.WriteLine($"Common letters: {commonLetters}");


static string GetCommonLettersBetweenCorrectBoxIds(IReadOnlyList<string> input)
{
    for (int i = 0; i < input.Count; i++)
    {
        for (int j = i + 1; j < input.Count; j++)
        {
            if (DifferByOneCharacter(input[i], input[j], out int differentIndex))
            {
                return input[i].Remove(differentIndex, 1);
            }
        }
    }

    throw new InvalidOperationException("No pair of Ids found that differ by only one character");
}

static bool DifferByOneCharacter(string first, string second, out int differentIndex)
{
    if (first.Length != second.Length)
        throw new ArgumentException($"Input strings must be the same length, were {first.Length} and {second.Length}");

    int differenceCount = 0;
    differentIndex = 0;
    for (int i = 0; i < first.Length; i++)
    {
        if (first[i] != second[i])
        {
            differenceCount++;
            differentIndex = i;
            if (differenceCount >= 2)
                return false;
        }
    }

    return differenceCount == 1;
}