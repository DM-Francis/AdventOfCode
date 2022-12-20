namespace Day19;

public record struct FactoryState
{
    public static readonly Dictionary<(FactoryState State, int Minutes), int> ScoreCache = new();

    public int Ore { get; init; }
    public int Clay { get; init; }
    public int Obsidian { get; init; }
    public int Geodes { get; init; }
    
    public int OreRobots { get; init; }
    public int ClayRobots { get; init; }
    public int ObsidianRobots { get; init; }
    public int GeodeRobots { get; init; }

    public bool FailedToMeetCost { get; private set; }

    public FactoryState AdvanceOneMinute()
    {
        return this with
        {
            Ore = Ore + OreRobots,
            Clay = Clay + ClayRobots,
            Obsidian = Obsidian + ObsidianRobots,
            Geodes = Geodes + GeodeRobots
        };
    }

    public FactoryState BuildOreRobot()
    {
        return this with { OreRobots = OreRobots + 1 };
    }
    
    public FactoryState BuildClayRobot()
    {
        return this with { ClayRobots = ClayRobots + 1 };
    }
    
    public FactoryState BuildObsidianRobot()
    {
        return this with { ObsidianRobots = ObsidianRobots + 1 };
    }
    
    public FactoryState BuildGeodeRobot()
    {
        return this with { GeodeRobots = GeodeRobots + 1 };
    }

    public bool CanMeetCost(Cost cost)
    {
        return Ore >= cost.Ore && Clay >= cost.Clay && Obsidian >= cost.Obsidian;
    }

    public FactoryState PayCost(Cost cost)
    {
        if (!CanMeetCost(cost))
            FailedToMeetCost = true;

        return this with
        {
            Ore = Ore - cost.Ore,
            Clay = Clay - cost.Clay,
            Obsidian = Obsidian - cost.Obsidian,
        };
    }
}