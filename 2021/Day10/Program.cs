var data = File.ReadAllLines("input.txt");


Dictionary<char, char> openToClose = new()
{
    { '(', ')' },
    { '[', ']' },
    { '{', '}' },
    { '<', '>' },
};

Dictionary<char, char> closeToOpen = openToClose.ToDictionary(kv => kv.Value, kv => kv.Key);


var illegalCharacters = data.Select(line => GetFirstIllegalCharacter(line)).Where(ch => ch is not null);

int totalSyntaxErrorScore = illegalCharacters.Sum(ch =>
{
    return ch switch
    {
        ')' => 3,
        ']' => 57,
        '}' => 1197,
        '>' => 25137,
        _ => 0
    };
});

Console.WriteLine($"Total syntax error score: {totalSyntaxErrorScore}");

var completionStrings = new List<string>();
foreach(string line in data)
{
    if (!IsIllegal(line))
    {
        string completionString = GetCompletionString(line);
        completionStrings.Add(completionString);
    }
}

List<long> orderedScores = completionStrings.Select(s => GetCompletionStringScore(s)).OrderBy(s => s).ToList();

long middleScore = orderedScores[orderedScores.Count / 2];

Console.WriteLine($"Middle score is {middleScore}");


bool IsIllegal(string line) => GetFirstIllegalCharacter(line) != null;

char? GetFirstIllegalCharacter(string line)
{
    var openChunks = new Stack<char>();

    foreach (char ch in line)
    {
        if (openToClose.ContainsKey(ch))
        {
            openChunks.Push(ch);
        }
        else if (openChunks.Peek() == closeToOpen[ch])
        {
            openChunks.Pop();
        }
        else
        {
            return ch;
        }
    }

    return null;
}

string GetCompletionString(string line)
{
    var openChunks = new Stack<char>();

    foreach (char ch in line)
    {
        if (openToClose.ContainsKey(ch))
        {
            openChunks.Push(ch);
        }
        else if (openChunks.Peek() == closeToOpen[ch])
        {
            openChunks.Pop();
        }
        else
        {
            throw new ArgumentException($"Line is invalid. Expected {openToClose[openChunks.Peek()]}, but found {ch}", nameof(line));
        }
    }

    char[] closingValues = openChunks.Select(x => openToClose[x]).ToArray();

    return new string(closingValues);
}

static long GetCompletionStringScore(string completionString)
{
    long score = 0;

    foreach (char ch in completionString)
    {
        score *= 5;
        score += ch switch
        {
            ')' => 1,
            ']' => 2,
            '}' => 3,
            '>' => 4,
            _ => throw new InvalidOperationException($"Invalid character in completion string: {ch}")
        };
    }

    return score;
}