char[] segments = new[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g' };
Dictionary<string, int> standardPatternToDigitMapping = new Dictionary<string, int>
{
    {"abcefg", 0 },
    {"cf", 1 },
    {"acdeg", 2 },
    {"acdfg", 3 },
    {"bcdf", 4 },
    {"abdfg", 5 },
    {"abdefg", 6 },
    {"acf", 7 },
    {"abcdefg", 8 },
    {"abcdfg", 9 },
};


PartOne();
PartTwo();


static void PartOne()
{
    var data = File.ReadAllLines("input.txt");

    int simpleDigitCount = 0;
    foreach (string line in data)
    {
        var lineParts = line.Split('|', StringSplitOptions.TrimEntries);

        string outputData = lineParts[1];
        var outputDataParts = outputData.Split(' ');

        foreach (string digitData in outputDataParts)
        {
            if (digitData.Length is 2 or 4 or 3 or 7) // Digits 1, 4, 7, 8
                simpleDigitCount++;
        }
    }

    Console.WriteLine(simpleDigitCount);
}

void PartTwo()
{
    var data = File.ReadAllLines("input.txt");

    var allOutputValues = new List<int>();

    foreach (string line in data)
    {
        var lineParts = line.Split('|', StringSplitOptions.TrimEntries);
        string uniquePatternsData = lineParts[0];
        var uniquePatterns = uniquePatternsData.Split(' ');

        string outputData = lineParts[1];
        var outputDigits = outputData.Split(' ');

        var mapping = CalculateMappingFromPatterns(uniquePatterns);

        var outputValueDigits = new List<int>();
        foreach(string digitPattern in outputDigits)
        {
            string standardPattern = MapToStandardSegments(digitPattern, mapping);
            int digit = standardPatternToDigitMapping[standardPattern];
            outputValueDigits.Add(digit);
        }

        int outputValue = int.Parse(string.Join("", outputValueDigits));
        allOutputValues.Add(outputValue);
    }

    Console.WriteLine(allOutputValues.Sum());
}

Dictionary<char, char> CalculateMappingFromPatterns(string[] uniquePatterns)
{
    var map = new Dictionary<char, char>();

    string one = uniquePatterns.Single(x => x.Length == 2);
    string four = uniquePatterns.Single(x => x.Length == 4);
    string seven = uniquePatterns.Single(x => x.Length == 3);
    string eight = uniquePatterns.Single(x => x.Length == 7);

    map['a'] = seven.Single(x => !one.Contains(x));
    char[] cf = one.ToCharArray();
    char[] bd = four.Where(x => !one.Contains(x)).ToArray();

    string[] length5Patterns = uniquePatterns.Where(x => x.Length == 5).ToArray();
    char[] adg = GetCommonCharacters(length5Patterns);
    char[] dg = adg.Where(x => x != map['a']).ToArray();

    map['d'] = GetCommonCharacters(new[] { new string(dg), new string(bd) })[0];
    map['b'] = bd.Single(x => x != map['d']);
    map['g'] = dg.Single(x => x != map['d']);

    string zero = uniquePatterns.Single(x => x.Length == 6 && !x.Contains(map['d']));
    string nine = uniquePatterns.Single(x => x.Length == 6 && x.Contains(cf[0]) && x.Contains(cf[1]) && x != zero);
    string six = uniquePatterns.Single(x => x.Length == 6 && x != nine && x != zero);

    map['e'] = segments.Single(x => !nine.Contains(x));
    map['c'] = segments.Single(x => !six.Contains(x));
    map['f'] = cf.Single(x => x != map['c']);

    return map;
}

static char[] GetCommonCharacters(string[] strings)
{
    string first = strings[0];

    return first.Where(x => strings.All(v => v.Contains(x))).ToArray();
}

static string MapToStandardSegments(string digit, IDictionary<char,char> mapping)
{
    var reverseMap = mapping.ToDictionary(x => x.Value, x => x.Key);

    var chars = new List<char>();
    foreach (char c in digit)
    {
        chars.Add(reverseMap[c]);
    }

    chars.Sort();

    return new string(chars.ToArray());
}