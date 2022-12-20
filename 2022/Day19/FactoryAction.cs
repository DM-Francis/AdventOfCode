namespace Day19;

public readonly record struct FactoryAction(string Label,
    Func<FactoryState, Blueprint, FactoryState> Buy,
    Func<FactoryState, FactoryState> Build)
{
    public static FactoryAction BuyGeodeRobot { get; } =
        new(nameof(BuyGeodeRobot), (f,b) => f.PayCost(b.GeodeRobotCost), f => f.BuildGeodeRobot());
    
    public static FactoryAction BuyObsidianRobot { get; } =
        new(nameof(BuyObsidianRobot), (f,b) => f.PayCost(b.ObsidianRobotCost), f => f.BuildObsidianRobot());

    public static FactoryAction BuyClayRobot { get; } =
        new(nameof(BuyClayRobot), (f,b) => f.PayCost(b.ClayRobotCost), f => f.BuildClayRobot());
    
    public static FactoryAction BuyOreRobot { get; } =
        new(nameof(BuyOreRobot), (f,b) => f.PayCost(b.OreRobotCost), f => f.BuildOreRobot());

    public static FactoryAction DoNothing { get; } = new(nameof(DoNothing), (f,b) => f, f => f);
    
    public static List<FactoryAction> AllActions { get; } = new()
    {
        BuyGeodeRobot,
        BuyObsidianRobot,
        BuyClayRobot,
        BuyOreRobot,
        DoNothing
    };
}