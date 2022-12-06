var input = File.ReadAllText("input.txt");

// Part 1
int firstMarker = GetFirstMarker(input, 4);
Console.WriteLine($"First marker location: {firstMarker}");

// Part 2
int startOfMessageMarker = GetFirstMarker(input, 14);
Console.WriteLine($"First start of message marker location: {startOfMessageMarker}");

static int GetFirstMarker(string input, int distinctRequired)
{
    var buffer = new Queue<char>();

    for (int i = 0; i < input.Length; i++)
    {
        char ch = input[i];
        buffer.Enqueue(ch);

        if (buffer.Count > distinctRequired)
            buffer.Dequeue();

        if (buffer.Count == distinctRequired && AreAllDistinct(buffer))
            return i + 1;
    }

    throw new InvalidOperationException("No marker found");
}

static bool AreAllDistinct(IReadOnlyCollection<char> chars)
{
    return chars.Distinct().Count() == chars.Count;
}
