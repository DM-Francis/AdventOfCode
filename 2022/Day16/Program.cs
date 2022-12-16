using Day16;
using Action = Day16.Action;

var input = File.ReadAllLines("input.txt");
// var input = """
// Valve AA has flow rate=0; tunnels lead to valves DD, II, BB
// Valve BB has flow rate=13; tunnels lead to valves CC, AA
// Valve CC has flow rate=2; tunnels lead to valves DD, BB
// Valve DD has flow rate=20; tunnels lead to valves CC, AA, EE
// Valve EE has flow rate=3; tunnels lead to valves FF, DD
// Valve FF has flow rate=0; tunnels lead to valves EE, GG
// Valve GG has flow rate=0; tunnels lead to valves FF, HH
// Valve HH has flow rate=22; tunnel leads to valve GG
// Valve II has flow rate=0; tunnels lead to valves AA, JJ
// Valve JJ has flow rate=21; tunnel leads to valve II
// """.Split("\r\n");


var graph = new Dictionary<string, HashSet<string>>();
var valves = new Dictionary<string, Valve>();

foreach (var line in input)
{
    var split = line.Split(new[] { ' ', ',', '=', ';' }, StringSplitOptions.RemoveEmptyEntries);
    var node = split[1];
    var valve = new Valve(int.Parse(split[5]));

    var adjacentNodes = split[10..];
    graph.Add(node, adjacentNodes.ToHashSet());
    valves.Add(node, valve);
}

int nodeCount = 0;
int maxTotalFlowRate = valves.Values.Sum(v => v.FlowRate);
int maximumPressureReleased = GetMaximumPressureReleasedFromNodeWithinTime("AA", null, 30, 0, 0, 0, graph, valves, ref nodeCount, maxTotalFlowRate);

Console.WriteLine($"Total nodes explored: {nodeCount}");
Console.WriteLine($"Maximum pressure released in 30 minutes: {maximumPressureReleased}");

static int GetMaximumPressureReleasedFromNodeWithinTime(string node, string? previousNode, int time, int flowRate, int releasedSoFar, int currentMax,
    Dictionary<string, HashSet<string>> graph, Dictionary<string, Valve> valves, ref int nodeCount, int maxTotalFlowRate)
{
    if (time == 0)
        return releasedSoFar;

    if (flowRate == maxTotalFlowRate)
        return releasedSoFar + maxTotalFlowRate * time;

    releasedSoFar += flowRate;

    int upperBoundInCurrentState = releasedSoFar + maxTotalFlowRate * time;
    if (upperBoundInCurrentState < currentMax)
        return currentMax;

    nodeCount++;
    
    var valve = valves[node];
    if (valve is { FlowRate: > 0, IsOpen: false })
    {
        valves[node] = valve with { IsOpen = true };
        int newMax = GetMaximumPressureReleasedFromNodeWithinTime(node, null, time - 1, flowRate + valve.FlowRate, releasedSoFar,
            currentMax, graph, valves, ref nodeCount, maxTotalFlowRate);

        if (newMax > currentMax)
            currentMax = newMax;

        valves[node] = valve;
    }

    foreach (string adjacentNode in graph[node].Where(adj => adj != previousNode))
    {
        int newMax =
            GetMaximumPressureReleasedFromNodeWithinTime(adjacentNode, node, time - 1, flowRate, releasedSoFar, currentMax, graph,
                valves, ref nodeCount, maxTotalFlowRate);

        if (newMax > currentMax)
            currentMax = newMax;
    }

    if (nodeCount % 10000 == 0)
        Console.WriteLine($"Nodes explored: {nodeCount}");
    return currentMax;
}

static IEnumerable<Action> GetAvailableActionsAtNode(string node, Action previousAction, Dictionary<string, HashSet<string>> graph,
    Dictionary<string, Valve> valves)
{
    if (valves[node] is { FlowRate: > 0, IsOpen: false })
        yield return new Action(ActionType.OpenValve, node);

    string? previousNode = previousAction.Type == ActionType.Move ? previousAction.Node : null;
    foreach (string adjacentNode in graph[node].Where(adj => adj != previousNode))
        yield return new Action(ActionType.Move, node);
}

namespace Day16
{
    public record struct Valve(int FlowRate, bool IsOpen = false);
    public record struct Action(ActionType Type, string Node);
    public enum ActionType { Move, OpenValve }
}