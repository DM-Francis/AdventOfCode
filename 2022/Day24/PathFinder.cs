namespace Day24;

public readonly record struct Node(Position P, int T);

public static class PathFinder
{
    public static int FindPathThroughValley(Valley valley)
    {
        return FindPathThroughValleyBetweenPositions(valley, valley.Start, valley.Finish, 0);
    }

    public static int FindPathThroughValleyBetweenPositions(Valley valley, Position start, Position finish, int startTime)
    {
        var nodeQueue = new Queue<Node>();
        var visited = new HashSet<Node>();
        var startNode = new Node(start, startTime);
        nodeQueue.Enqueue(startNode);
        visited.Add(startNode);

        while (nodeQueue.Count > 0)
        {
            var current = nodeQueue.Dequeue();
            if (current.P == finish)
                return current.T;
            
            var options = GetOptionsAt(current.P, valley, current.T + 1);
            foreach (var option in options)
            {
                if (visited.Contains(option))
                    continue;
                nodeQueue.Enqueue(option);
                visited.Add(option);
            }
        }

        throw new Exception("Could not find a path through the valley.");
    }

    private static IEnumerable<Node> GetOptionsAt(Position p, Valley valley, int t)
    {
        while (valley.Time < t)
        {
            valley.Advance1TimeStep();
            Console.WriteLine($"Valley time: {valley.Time}");
        }

        var options = new List<Node>();
        Span<Position> candidates = stackalloc Position[5];
        candidates[0] = p;
        candidates[1] = p with { Y = p.Y - 1 };
        candidates[2] = p with { Y = p.Y + 1 };
        candidates[3] = p with { X = p.X - 1 };
        candidates[4] = p with { X = p.X + 1 };

        foreach (var candidate in candidates)
        {
            if (candidate.X < 0 || candidate.X >= valley.Width)
                continue;
            if (candidate.Y < 0 || candidate.Y >= valley.Height)
                continue;
            if (valley.Walls[candidate.X, candidate.Y])
                continue;
            if (valley.Blizzards[candidate.X, candidate.Y].Count > 0)
                continue;
            options.Add(new Node(candidate, t));
        }

        return options;
    }
}