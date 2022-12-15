var input = File.ReadAllLines("input.txt");

var initialStateString = input[0].Split(' ')[2];
var rules = input[2..].Select(GetRuleFromString).ToDictionary(x => x.State, x => x.WillHavePlant);

// Part 1
var potStates = GetInitialStateFromString(initialStateString);

var finalStates = AdvanceNGenerations(potStates, rules, 20);
long sumOfPotsNumbersWithPlants = GetPotNumberSum(finalStates);

Console.WriteLine($"Sum of pot numbers with plants after 20 generations: {sumOfPotsNumbersWithPlants}");

// Part 2
var potStates2 = GetInitialStateFromString(initialStateString);
long finalSum = GetSumAfterNGenerations(potStates2, rules, 50_000_000_000);
// Seems to always increase by a constant amount with each generation (after a certain point)

Console.WriteLine($"Sum of pot numbers after 50 billion generations: {finalSum}");

static Dictionary<long, bool> AdvanceNGenerations(Dictionary<long, bool> potStates, Dictionary<FivePotState, bool> rules, long n)
{
    long previousSum = 0;
    for (long i = 0; i < n; i++)
    {
        if (i % 10000 == 0)
        {
            long potNumberSum = GetPotNumberSum(potStates);
            Console.WriteLine($"On generation {i}. Sum: {potNumberSum} ({potNumberSum - previousSum})");
            previousSum = potNumberSum;
        }
        
        potStates = AdvanceOneGeneration(potStates, rules);
    }

    return potStates;
}

static Dictionary<long, bool> AdvanceOneGeneration(Dictionary<long, bool> potStates, Dictionary<FivePotState, bool> rules)
{
    var newPotStates = new Dictionary<long, bool>();
    long minPotNumber = potStates.Where(x => x.Value).Min(x => x.Key);
    long maxPotNumber = potStates.Where(x => x.Value).Max(x => x.Key);

    long start = minPotNumber - 2;
    long finish = maxPotNumber + 2;

    for (long i = start; i <= finish; i++)
    {
        bool l2 = HasPlantInPotNumber(potStates, i - 2);
        bool l1 = HasPlantInPotNumber(potStates, i - 1);
        bool c = HasPlantInPotNumber(potStates, i);
        bool r1 = HasPlantInPotNumber(potStates, i + 1);
        bool r2 = HasPlantInPotNumber(potStates, i + 2);

        var fivePotState = new FivePotState(l2, l1, c, r1, r2);
        bool willHavePlant = rules[fivePotState];

        if (willHavePlant)
            newPotStates[i] = willHavePlant;
    }

    return newPotStates;
}

static long GetSumAfterNGenerations(Dictionary<long, bool> potStates, Dictionary<FivePotState, bool> rules, long n)
{
    if (n < 10000)
        throw new ArgumentOutOfRangeException(nameof(n), "n must be greater than 10,000");
    
    potStates = AdvanceNGenerations(potStates, rules, 9999); // Will have a stable delta after 10,000 gens
    long sumAt9999 = GetPotNumberSum(potStates);
    potStates = AdvanceOneGeneration(potStates, rules);
    long sumAt10000 = GetPotNumberSum(potStates);

    long delta = sumAt10000 - sumAt9999;
    long remainingGenerations = n - 10000;
    
    return sumAt10000 + delta * remainingGenerations;
}

static bool HasPlantInPotNumber(Dictionary<long, bool> potStates, long potNumber)
{
    if (potStates.TryGetValue(potNumber, out bool hasPlant))
        return hasPlant;

    return false;
}

static long GetPotNumberSum(Dictionary<long, bool> potStates) => potStates.Where(x => x.Value).Sum(x => x.Key);

static Dictionary<long, bool> GetInitialStateFromString(string value)
{
    var potStates = new Dictionary<long, bool>();
    for (int i = 0; i < value.Length; i++)
    {
        if (value[i] == '#')
            potStates[i] = true;
        else
            potStates[i] = false;
    }

    return potStates;
}

static (FivePotState State, bool WillHavePlant) GetRuleFromString(string value)
{
    var split = value.Split(' ');
    var stateString = split[0];

    var fivePotState = new FivePotState(
        stateString[0] == '#',
        stateString[1] == '#',
        stateString[2] == '#',
        stateString[3] == '#',
        stateString[4] == '#');

    var resultString = split[2];
    var willHavePlant = resultString == "#";

    return (fivePotState, willHavePlant);
}


public record struct FivePotState(bool L2, bool L1, bool C, bool R1, bool R2);