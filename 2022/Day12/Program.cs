using Day12;

var input = File.ReadAllLines("input.txt");
var map = GetMapFromInput(input);

// Part 1
int distanceFromStart = GetShortestDistanceToEnd(map, SearchType.FromStart);
Console.WriteLine($"Shortest number of steps from start: {distanceFromStart}");

// Part 2
int distanceFromLowGround = GetShortestDistanceToEnd(map, SearchType.FromAnyLowestHeight);
Console.WriteLine($"Shortest number of steps from any low ground: {distanceFromLowGround}");

static int GetShortestDistanceToEnd(Map map, SearchType searchType)
{
    var routes = new Dictionary<Position, Position>();
    var distances = new Dictionary<Position, int>();
    var toExplore = new Queue<Position>();

    distances.Add(map.BestSignalPosition, 0);
    toExplore.Enqueue(map.BestSignalPosition);

    while (toExplore.Count > 0)
    {
        var current = toExplore.Dequeue();
        if (searchType == SearchType.FromStart && current == map.StartingPosition
            || searchType == SearchType.FromAnyLowestHeight && map.Heights[current.X, current.Y] == 0)
        {
            RenderRouteMap(routes, map, current);
            return distances[current];
        }

        foreach (var next in map.GetReverseAccessiblePositionsFrom(current))
        {
            if (distances.ContainsKey(next))
                continue;

            distances[next] = distances[current] + 1;
            routes[next] = current;
            toExplore.Enqueue(next);
        }
    }

    throw new InvalidOperationException("Did not reach starting position");
}

static Map GetMapFromInput(string[] input)
{
    var xSize = input[0].Length;
    var ySize = input.Length;
    var grid = new int[xSize, ySize];
    Position startingPosition = default;
    Position bestSignalPosition = default;

    for (int x = 0; x < xSize; x++)
    {
        for (int y = 0; y < ySize; y++)
        {
            char value = input[y][x];
            grid[x, y] = CharToHeight(value);
            if (value == 'S')
                startingPosition = new Position(x, y);
            else if (value == 'E')
                bestSignalPosition = new Position(x, y);
        }
    }

    return new Map(grid, startingPosition, bestSignalPosition);
}

static int CharToHeight(char c)
{
    return c switch
    {
        'S' => 0,
        'E' => 25,
        _ => c - 'a'
    };
}

static void RenderRouteMap(IReadOnlyDictionary<Position, Position> routes, Map map, Position startingPosition)
{
    var current = startingPosition;
    var fullRoute = new HashSet<Position> { current };

    while (routes.TryGetValue(current, out var next))
    {
        fullRoute.Add(next);
        current = next;
    }
    
    for (int y = 0; y <= map.MaxY; y++)
    {
        for (int x = 0; x <= map.MaxX; x++)
        {
            if (fullRoute.Contains(new Position(x, y)))
                Console.ForegroundColor = ConsoleColor.Red;
            
            Console.Write((char)(map.Heights[x, y] + 'a'));
            Console.ResetColor();
        }
        
        Console.WriteLine();
    }
}

public enum SearchType { FromStart, FromAnyLowestHeight }