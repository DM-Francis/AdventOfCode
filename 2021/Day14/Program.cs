using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

var data = File.ReadAllLines("input.txt");
var testData = new string[]
{
    "NNCB",
    "",
    "CH -> B",
    "HH -> N",
    "CB -> H",
    "NH -> C",
    "HB -> C",
    "HC -> B",
    "HN -> C",
    "NN -> C",
    "BH -> H",
    "NC -> B",
    "NB -> B",
    "BN -> B",
    "BB -> N",
    "BC -> B",
    "CC -> N",
    "CN -> C",
};

string template = data[0];
string[] ruleData = data[2..];

var rules = ruleData.ToDictionary(x => x[..2], x => x[6]);

//PartOne();
PartTwo();


void PartOne()
{
    var polymer = template.ToList();
    Console.WriteLine($"Starting polymer length is: {polymer.Count}");

    for (int i = 0; i < 10; i++)
    {
        Console.WriteLine($"Running step {i + 1}...");
        ApplyStepToPolymer(polymer, rules);
        Console.WriteLine($"New polymer length is: {polymer.Count}");
    }

    int mostCommonCount = polymer.GroupBy(x => x).Max(g => g.Count());
    int leastCommonCount = polymer.GroupBy(x => x).Min(g => g.Count());

    Console.WriteLine(mostCommonCount - leastCommonCount);
}

void PartTwo()
{
    Console.WriteLine("Attempting fast method");

    var rules = ruleData.ToDictionary(x => new Pair(x[0], x[1]), x => x[6]);

    int steps = 40;
    var polymer = template.ToList();
    var counts = GetCountsForPolymerAfterSteps(polymer, rules, steps);

    Console.WriteLine($"Done {steps} steps.");

    long mostCommonCount = counts.Values.Max();
    long leastCommonCount = counts.Values.Min();

    Console.WriteLine(mostCommonCount - leastCommonCount);
}

static void ApplyStepToPolymer(List<char> polymer, Dictionary<string, char> rules)
{
    var initialLength = polymer.Count;

    for (int i = 0; i < initialLength - 1; i++)
    {
        char element1 = polymer[2 * i];
        char element2 = polymer[2 * i + 1];
        string pair = new(new[] { element1, element2 });

        char toInsert = rules[pair];

        polymer.Insert(2 * i + 1, toInsert);
    }
}

static Dictionary<char,long> GetCountsForPolymerAfterSteps(List<char> polymer, Dictionary<Pair, char> rules, int steps)
{
    var initialPairs = new List<Pair>();

    for (int i = 0; i < polymer.Count - 1; i++)
    {
        initialPairs.Add(new Pair(polymer[i], polymer[i + 1]));
    }

    var validChars = rules.Values.Distinct().ToList();
    var overallCounts = new Counts(validChars);
    var countCache = new Dictionary<(Pair pair, int steps), Counts>();

    for (int i = 0; i < initialPairs.Count; i++)
    {
        overallCounts += GetCountsForPairAfterSteps(initialPairs[i], rules, steps, validChars, countCache);
        Console.WriteLine($"Completed pair {i + 1}/{initialPairs.Count}");
    }

    char firstChar = polymer[0];
    char lastChar = polymer[polymer.Count - 1];

    var finalCounts = new Dictionary<char,long>();

    foreach (char element in overallCounts.Keys)
    {
        if (element == firstChar || element == lastChar)
            finalCounts[element] = overallCounts[element] / 2 + 1;
        else
            finalCounts[element] = overallCounts[element] / 2;
    }

    return finalCounts;
}

static Counts GetCountsForPairAfterSteps(Pair pair, Dictionary<Pair,char> rules, int steps, IEnumerable<char> validChars, Dictionary<(Pair pair, int steps), Counts> countCache)
{
    if (countCache.TryGetValue((pair, steps), out Counts? countsFromCache))
    {
        return countsFromCache;
    }

    if (steps == 0)
    {
        return new Counts(validChars, pair);
    }

    var (pair1, pair2) = GetNewPairs(pair, rules);
    var counts1 = GetCountsForPairAfterSteps(pair1, rules, steps - 1, validChars, countCache);
    var counts2 = GetCountsForPairAfterSteps(pair2, rules, steps - 1, validChars, countCache);

    countCache[(pair, steps)] = counts1 + counts2;

    return counts1 + counts2;
}

static (Pair Pair1, Pair Pair2) GetNewPairs(Pair pair, Dictionary<Pair,char> rules)
{
    char toInsert = rules[pair];
    var pair1 = new Pair(pair.A, toInsert);
    var pair2 = new Pair(toInsert, pair.B);

    return (pair1, pair2);
}


readonly struct Pair
{
    public readonly char A;
    public readonly char B;

    public Pair(char a, char b)
    {
        A = a;
        B = b;
    }
}

class Counts : IDictionary<char, long>
{
    private readonly Dictionary<char, long> _counts = new();


    public Counts(IEnumerable<char> validChars)
    {
        foreach (char c in validChars)
        {
            _counts[c] = 0;
        }
    }

    public Counts(IEnumerable<char> validChars, Dictionary<char, long> initial)
    {
        foreach(char c in validChars)
        {
            _counts[c] = 0;
        }

        foreach(var key in initial.Keys)
        {
            _counts[key] = initial[key];
        }
    }

    public Counts(IEnumerable<char> validChars, Pair basePair)
    {
        foreach(char c in validChars)
        {
            _counts[c] = 0;
        }

        if (basePair.A == basePair.B)
        {
            _counts[basePair.A] = 2;
        }
        else
        {
            _counts[basePair.A] = 1;
            _counts[basePair.B] = 1;
        }
    }

    public long this[char key]
    {
        get => _counts[key];
        set => _counts[key] = value;
    }

    public Counts Clone()
    {
        return new Counts(_counts.Keys, _counts);
    }

    public static Counts operator +(Counts left, Counts right)
    {
        var newCounts = left.Clone();

        foreach (char element in right.Keys)
        {
            newCounts[element] += right[element];
        }

        return newCounts;
    }

    public ICollection<char> Keys => ((IDictionary<char, long>)_counts).Keys;

    public ICollection<long> Values => ((IDictionary<char, long>)_counts).Values;

    public int Count => ((ICollection<KeyValuePair<char, long>>)_counts).Count;

    public bool IsReadOnly => ((ICollection<KeyValuePair<char, long>>)_counts).IsReadOnly;

    public void Add(char key, long value)
    {
        ((IDictionary<char, long>)_counts).Add(key, value);
    }

    public void Add(KeyValuePair<char, long> item)
    {
        ((ICollection<KeyValuePair<char, long>>)_counts).Add(item);
    }

    public void Clear()
    {
        ((ICollection<KeyValuePair<char, long>>)_counts).Clear();
    }

    public bool Contains(KeyValuePair<char, long> item)
    {
        return ((ICollection<KeyValuePair<char, long>>)_counts).Contains(item);
    }

    public bool ContainsKey(char key)
    {
        return ((IDictionary<char, long>)_counts).ContainsKey(key);
    }

    public void CopyTo(KeyValuePair<char, long>[] array, int arrayIndex)
    {
        ((ICollection<KeyValuePair<char, long>>)_counts).CopyTo(array, arrayIndex);
    }

    public IEnumerator<KeyValuePair<char, long>> GetEnumerator()
    {
        return ((IEnumerable<KeyValuePair<char, long>>)_counts).GetEnumerator();
    }

    public bool Remove(char key)
    {
        return ((IDictionary<char, long>)_counts).Remove(key);
    }

    public bool Remove(KeyValuePair<char, long> item)
    {
        return ((ICollection<KeyValuePair<char, long>>)_counts).Remove(item);
    }

    public bool TryGetValue(char key, [MaybeNullWhen(false)] out long value)
    {
        return ((IDictionary<char, long>)_counts).TryGetValue(key, out value);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_counts).GetEnumerator();
    }
}