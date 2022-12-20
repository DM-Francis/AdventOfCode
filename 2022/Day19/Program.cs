using Day19;

var input = File.ReadAllLines("input.txt");
var exampleInput = """
Blueprint 1: Each ore robot costs 4 ore.  Each clay robot costs 2 ore.  Each obsidian robot costs 3 ore and 14 clay.  Each geode robot costs 2 ore and 7 obsidian.
Blueprint 2:  Each ore robot costs 2 ore.  Each clay robot costs 3 ore.  Each obsidian robot costs 3 ore and 8 clay.  Each geode robot costs 3 ore and 12 obsidian.
""".Split(Environment.NewLine);

var blueprints = new List<Blueprint>();
foreach (var line in input)
{
    var split = line.Split(new [] {' ', ':'}, StringSplitOptions.RemoveEmptyEntries);
    int id = int.Parse(split[1]);
    int oreRobotCostInOre = int.Parse(split[6]);
    int clayRobotCostInOre = int.Parse(split[12]);
    int obsidianRobotCostInOre = int.Parse(split[18]);
    int obsidianRobotCostInClay = int.Parse(split[21]);
    int geodeRobotCostInOre = int.Parse(split[27]);
    int geodeRobotCostInObsidian = int.Parse(split[30]);

    var blueprint = new Blueprint(
        id,
        new Cost(oreRobotCostInOre, 0, 0),
        new Cost(clayRobotCostInOre, 0, 0),
        new Cost(obsidianRobotCostInOre, obsidianRobotCostInClay, 0),
        new Cost(geodeRobotCostInOre, 0, geodeRobotCostInObsidian));
    
    blueprints.Add(blueprint);
}

// Part 1
var maxGeodesPerBlueprint = new Dictionary<int, int>(); // Keyed on blueprint id

foreach (var blueprint in blueprints)
{
    Console.WriteLine($"Checking blueprint {blueprint.Id}...");
    int maxGeodes = CalculateMaxGeodesUsingBlueprint(blueprint, 24);
    maxGeodesPerBlueprint.Add(blueprint.Id, maxGeodes);
    Console.WriteLine($"Max geodes for blueprint {blueprint.Id}: {maxGeodes}");
    FactoryState.ScoreCache.Clear();
}

foreach (var (blueprintId, max) in maxGeodesPerBlueprint)
{
    Console.WriteLine($"Max geodes for blueprint {blueprintId}: {max}");
}

int sumOfQualityLevels = maxGeodesPerBlueprint.Sum(x => x.Key * x.Value);

Console.WriteLine($"Sum of quality levels: {sumOfQualityLevels}");


// Part 2
var maxGeodesPerBlueprint32Mins = new Dictionary<int, int>();
foreach (var blueprint in blueprints.Take(3))
{
    Console.WriteLine($"Checking blueprint {blueprint.Id}...");
    int maxGeodes = CalculateMaxGeodesUsingBlueprint(blueprint, 32);
    maxGeodesPerBlueprint32Mins.Add(blueprint.Id, maxGeodes);
    Console.WriteLine($"Max geodes for blueprint {blueprint.Id}: {maxGeodes}");
    FactoryState.ScoreCache.Clear();
}

int productOfMaxGeodes = maxGeodesPerBlueprint32Mins.Values.Aggregate(1, (agg, v) => agg * v);

Console.WriteLine($"Product of max geodes: {productOfMaxGeodes}");


static int CalculateMaxGeodesUsingBlueprint(Blueprint blueprint, int minutes)
{
    var factoryState = new FactoryState { OreRobots = 1 };
    int nodeCount = 0;
    var bestActions = new Dictionary<int, string>();
    int maxGeodes = GetMaxGeodesFromState(blueprint, factoryState, minutes, 0, ref nodeCount, bestActions);

    foreach (var (minute, action) in bestActions.OrderByDescending(x => x.Key))
    {
        Console.WriteLine($"Minutes {minutes + 1 - minute}: {action}");
    }

    return maxGeodes;
}

static int GetMaxGeodesFromState(Blueprint blueprint, FactoryState factoryState, int minutesRemaining, int currentMax, ref int nodeCount, IDictionary<int, string> bestActions)
{
    nodeCount++;
    
    if (minutesRemaining == 0)
        return factoryState.Geodes;

    int upperBoundFromCurrentState = factoryState.Geodes + minutesRemaining * factoryState.GeodeRobots + minutesRemaining * (minutesRemaining + 1) / 2;
    if (upperBoundFromCurrentState <= currentMax)
        return currentMax;

    foreach (var action in FactoryAction.AllActions)
    {
        var newState = action.Buy(factoryState, blueprint);
        if (newState.FailedToMeetCost)
            continue;
        
        newState = newState.AdvanceOneMinute();
        newState = action.Build(newState);

        int newMax;
        if (FactoryState.ScoreCache.TryGetValue((newState, minutesRemaining), out int maxFromCache))
        {
            newMax = maxFromCache;
        }
        else
        {
            newMax = GetMaxGeodesFromState(blueprint, newState, minutesRemaining - 1, currentMax, ref nodeCount, bestActions);
            FactoryState.ScoreCache[(newState, minutesRemaining)] = newMax;
        }

        if (newMax > currentMax)
        {
            currentMax = newMax;
            bestActions[minutesRemaining] = action.Label;
        }
    }
    
    if (nodeCount % 1000000 == 0)
        Console.WriteLine($"{blueprint.Id} node count: {nodeCount:#,#}  Current max: {currentMax}");

    return currentMax;
}

public record Blueprint(
    int Id,
    Cost OreRobotCost,
    Cost ClayRobotCost,
    Cost ObsidianRobotCost,
    Cost GeodeRobotCost);
    
public readonly record struct Cost(int Ore, int Clay, int Obsidian);