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


var graph = new Dictionary<string, Dictionary<string,int>>();
var valves = new Dictionary<string, Valve>();

foreach (var line in input)
{
    var split = line.Split(new[] { ' ', ',', '=', ';' }, StringSplitOptions.RemoveEmptyEntries);
    var node = split[1];
    var valve = new Valve(int.Parse(split[5]));

    var adjacentNodes = split[10..];
    graph.Add(node, adjacentNodes.ToDictionary(n => n, _ => 1));
    valves.Add(node, valve);
}

RemoveRedundantNodes(graph, valves);


// Part 1
int nodeCount = 0;
int maxTotalFlowRate = valves.Values.Sum(v => v.FlowRate);
var visited = new HashSet<string>();
int maximumPressureReleased = GetMaximumPressureReleasedFromNodeWithinTime("AA", visited, 0,30, 0, 0, 0, 0, graph, valves, ref nodeCount, maxTotalFlowRate);

Console.WriteLine($"Total nodes explored: {nodeCount:#,#}");
Console.WriteLine($"Maximum pressure released in 30 minutes: {maximumPressureReleased}");

// Part 2
nodeCount = 0;
foreach (var node in valves.Keys)
    valves[node] = valves[node] with { IsOpen = false };

visited.Clear();
var elephantVisited = new HashSet<string>();

int maximumPressureReleasedWithElephant = GetMaximumPressureReleasedFromNodeWithinTimeWithElephant(
    "AA", visited, 0,
    "AA", elephantVisited, 0,
    26, 0, 0, 0, 0, graph, valves, ref nodeCount, maxTotalFlowRate);

Console.WriteLine($"Total nodes explored: {nodeCount:#,#}");
Console.WriteLine($"Maximum pressure released in 26 minutes with elephant: {maximumPressureReleasedWithElephant}");


static int GetMaximumPressureReleasedFromNodeWithinTime(string node, HashSet<string> visitedSinceLastOpen, int remainingActionTime, int time, int elapsed, int flowRate, int releasedSoFar, int currentMax,
    Dictionary<string, Dictionary<string, int>> graph, Dictionary<string, Valve> valves, ref int nodeCount, int maxTotalFlowRate)
{
    time -= elapsed;
    remainingActionTime -= elapsed;

    if (time <= 0)
    {
        var previousTime = time + elapsed;
        return releasedSoFar + flowRate * (previousTime - 1);
    }

    if (flowRate == maxTotalFlowRate)
        return releasedSoFar + maxTotalFlowRate * time;

    releasedSoFar += flowRate * elapsed;

    int upperBoundInCurrentState = releasedSoFar + maxTotalFlowRate * time;
    if (upperBoundInCurrentState < currentMax)
        return currentMax;

    nodeCount++;
    
    foreach (var action in GetAvailableActionsAtNode(node, graph, valves, visitedSinceLastOpen, remainingActionTime))
    {
        flowRate += ApplyActionToValvesAndGetFlowRate(action, valves);
        var visited = action.Type == ActionType.OpenValve ? new HashSet<string>() : visitedSinceLastOpen;
        visited.Add(node);
        
        int newMax = GetMaximumPressureReleasedFromNodeWithinTime(action.TargetNode, visited, action.TimeToComplete, time, action.TimeToComplete, flowRate, releasedSoFar, currentMax, graph,
            valves, ref nodeCount, maxTotalFlowRate);

        visited.Remove(node);
        flowRate -= UndoActionAndGetFlowRate(action, valves);

        if (newMax > currentMax)
            currentMax = newMax;
    }

    if (nodeCount % 10000 == 0)
        Console.WriteLine($"Nodes explored: {nodeCount:#,#}");
    return currentMax;
}

static int GetMaximumPressureReleasedFromNodeWithinTimeWithElephant(string node, HashSet<string> visited, int remainingActionTime,
    string elephantNode, HashSet<string> elephantVisited, int elephantRemainingActionTime,
    int time, int elapsed, int flowRate, int releasedSoFar,
    int currentMax,
    Dictionary<string, Dictionary<string, int>> graph, Dictionary<string, Valve> valves, ref int nodeCount,
    int maxTotalFlowRate)
{
    time -= elapsed;
    remainingActionTime -= elapsed;
    elephantRemainingActionTime -= elapsed;
    
    if (time <= 0)
        return releasedSoFar;

    if (flowRate == maxTotalFlowRate)
        return releasedSoFar + maxTotalFlowRate * time;

    releasedSoFar += flowRate * elapsed;

    int upperBoundInCurrentState = releasedSoFar + maxTotalFlowRate * time;
    if (upperBoundInCurrentState < currentMax)
        return currentMax;

    nodeCount++;

    foreach (var action in GetAvailableActionsAtNode(node, graph, valves, visited, remainingActionTime))
    {
        flowRate += ApplyActionToValvesAndGetFlowRate(action, valves);
        var newVisited = action.Type == ActionType.OpenValve ? new HashSet<string>() : visited;
        newVisited.Add(node);
        
        foreach (var elephantAction in GetAvailableActionsAtNode(elephantNode, graph, valves, elephantVisited, elephantRemainingActionTime))
        {
            flowRate += ApplyActionToValvesAndGetFlowRate(elephantAction, valves);
            var newElephantVisited = elephantAction.Type == ActionType.OpenValve ? new HashSet<string>() : elephantVisited;
            newElephantVisited.Add(elephantNode);
            
            int newMax = GetMaximumPressureReleasedFromNodeWithinTimeWithElephant(action.TargetNode,
                newVisited,
                action.TimeToComplete,
                elephantAction.TargetNode,
                newElephantVisited,
                elephantAction.TimeToComplete,
                time,
                Math.Min(action.TimeToComplete, elephantAction.TimeToComplete),
                flowRate,
                releasedSoFar,
                currentMax,
                graph, valves, ref nodeCount, maxTotalFlowRate);

            newElephantVisited.Remove(elephantNode);
            flowRate -= UndoActionAndGetFlowRate(elephantAction, valves);

            if (newMax > currentMax)
                currentMax = newMax;
        }

        newVisited.Remove(node);
        flowRate -= UndoActionAndGetFlowRate(action, valves);
    }

    if (nodeCount % 100000 == 0)
        Console.WriteLine($"Nodes explored: {nodeCount:#,#}.  Current max {currentMax}");
    return currentMax;
}

static IEnumerable<Action> GetAvailableActionsAtNode(string node, Dictionary<string, Dictionary<string,int>> graph,
    Dictionary<string, Valve> valves, HashSet<string> visitedNodes, int remainingActionTime)
{
    if (remainingActionTime > 0)
    {
        yield return new Action(ActionType.Wait, node, node, remainingActionTime);
        yield break;
    }
    
    var valve = valves[node];
    if (valve.FlowRate > 0 && !valve.IsOpen)
        yield return new Action(ActionType.OpenValve, node, node);

    var adjacentNodes = graph[node].ToList();
    for (int i = 0; i < adjacentNodes.Count; i++)
    {
        var adjacentNode = adjacentNodes[i];
        if (visitedNodes.Contains(adjacentNode.Key))
            continue;

        yield return new Action(ActionType.Move, node, adjacentNode.Key, adjacentNode.Value);
    }
}

static int ApplyActionToValvesAndGetFlowRate(Action action, Dictionary<string, Valve> valves)
{
    return action.Type == ActionType.OpenValve ? SetValveActionAndGetFlowRate(action, valves, true) : 0;
}

static int UndoActionAndGetFlowRate(Action action, Dictionary<string, Valve> valves)
{
    return action.Type == ActionType.OpenValve ? SetValveActionAndGetFlowRate(action, valves, false) : 0;
}

static int SetValveActionAndGetFlowRate(Action action, Dictionary<string, Valve> valves, bool value)
{
    if (action.Type != ActionType.OpenValve) throw new ArgumentException("Must be an OpenValve action");

    var valve = valves[action.TargetNode];
    valves[action.TargetNode] = valve with { IsOpen = value };
    return valve.FlowRate;
}


static void RemoveRedundantNodes(Dictionary<string, Dictionary<string, int>> graph, Dictionary<string, Valve> valves)
{
    var nodes = graph.Keys.ToList();
    foreach (var node in nodes)
    {
        var adjacentNodes = graph[node].ToList();
        var valve = valves[node];
        if (valve.FlowRate == 0 && adjacentNodes.Count == 2 && node != "AA")
        {
            graph.Remove(node);
            valves.Remove(node);

            var nodeA = adjacentNodes[0];
            var nodeB = adjacentNodes[1];
            graph[nodeA.Key].Remove(node);
            graph[nodeB.Key].Remove(node);
            
            AddConnectionBetween2Nodes(nodeA, nodeB, graph);
        }
    }
}

static void AddConnectionBetween2Nodes(KeyValuePair<string,int> nodeA, KeyValuePair<string,int> nodeB, Dictionary<string, Dictionary<string, int>> graph)
{
    var newDistance = nodeA.Value + nodeB.Value;
    AddNewConnectionToNode(nodeA.Key, nodeB.Key, newDistance, graph);
    AddNewConnectionToNode(nodeB.Key, nodeA.Key, newDistance, graph);
}

static void AddNewConnectionToNode(string nodeName, string targetNode, int newDistance,
    Dictionary<string, Dictionary<string, int>> graph)
{
    var currentConnections = graph[nodeName];

    if (currentConnections.TryGetValue(targetNode, out int existingDistance))
    {
        if (existingDistance <= newDistance)
            return;
    }

    currentConnections[targetNode] = newDistance;
}



namespace Day16
{
    public record struct Valve(int FlowRate, bool IsOpen = false);
    public record struct Action(ActionType Type, string Node, string TargetNode, int TimeToComplete = 1);
    public enum ActionType { Move, OpenValve, Wait }
}