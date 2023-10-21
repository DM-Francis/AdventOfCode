namespace Day22;

public static class CubeFunctions
{
    public static Dictionary<Location, Location> GetEdgeMap(MapSquare[,] map)
    {
        var edges = GetEdgesFromMap(map);
        var edgeMap = new Dictionary<Location, Location>();
        var connected = new HashSet<Edge>();

        int rotations = 1;
        while (connected.Count < edges.Count)
        {
            ConnectValidAdjacentEdgesSinglePass(edges, connected, rotations++, edgeMap);
        }
        
        return edgeMap;
    }

    public static List<Edge> GetEdgesFromMap(MapSquare[,] map)
    {
        int edgeLength = FindEdgeLength(map);
        var edges = new List<Edge>();

        for (int x = 0; x < map.GetLength(0); x += edgeLength)
        {
            for (int y = 0; y < map.GetLength(1); y += edgeLength)
            {
                var face = new Position(x, y);
                if (map[face.X, face.Y] == MapSquare.Empty)
                    continue;
                foreach (var direction in new[] {Facing.Up, Facing.Left, Facing.Down, Facing.Right})
                {
                    var nextFace = face.MoveInDirection(direction, edgeLength);
                    if (IsEmptyOrOffMap(nextFace, map))
                    {
                        var edge = GetEdgeForFaceAndDirection(face, direction, edgeLength);
                        edges.Add(edge);
                    }
                }
            }
        }

        return OrderEdges(edges);
    }

    public static int FindEdgeLength(MapSquare[,] map)
    {
        int cubeSurfaceArea = 0;
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j] != MapSquare.Empty)
                    cubeSurfaceArea++;
            }
        }
        
        // cubeSurfaceArea = edgeLength ^ 2 * 6
        // =>  edgeLength = sqrt(cubeSurfaceArea / 6)
        var edgeLength = Math.Sqrt(cubeSurfaceArea / 6);
        return (int)edgeLength;
    }

    private static bool IsEmptyOrOffMap(Position position, MapSquare[,] map)
    {
        if (position.X < 0 || position.Y < 0)
            return true;
        if (position.X > map.GetUpperBound(0) || position.Y > map.GetUpperBound(1))
            return true;
        
        return map[position.X, position.Y] == MapSquare.Empty;
    }

    private static Edge GetEdgeForFaceAndDirection(Position topLeftOfFace, Facing direction, int edgeLength)
    {
        Position edgeStart;
        Position edgeEnd;
        switch(direction)
        {
            case Facing.Up:
                edgeStart = topLeftOfFace;
                edgeEnd = edgeStart.MoveInDirection(Facing.Right, edgeLength - 1);
                return new Edge(edgeStart, edgeEnd, direction);
            case Facing.Right:
                edgeStart = topLeftOfFace.MoveInDirection(Facing.Right, edgeLength - 1);
                edgeEnd = edgeStart.MoveInDirection(Facing.Down, edgeLength - 1);
                return new Edge(edgeStart, edgeEnd, direction);
            case Facing.Down:
                edgeEnd = topLeftOfFace.MoveInDirection(Facing.Down, edgeLength - 1);
                edgeStart = edgeEnd.MoveInDirection(Facing.Right, edgeLength - 1);
                return new Edge(edgeStart, edgeEnd, direction);
            case Facing.Left:
                edgeEnd = topLeftOfFace;
                edgeStart = edgeEnd.MoveInDirection(Facing.Down, edgeLength - 1);
                return new Edge(edgeStart, edgeEnd, direction);
            default:
                throw new ArgumentOutOfRangeException(nameof(direction));
        }
    }

    private static List<Edge> OrderEdges(IReadOnlyCollection<Edge> edges)
    {
        var firstEdge = edges.OrderBy(x => x.Start.Y).ThenBy(x => x.Start.X).First();
        var orderedEdges = new List<Edge> { firstEdge };
        for (int i = 0; i < edges.Count - 1; i++)
        {
            var current = orderedEdges[i];

            var next = GetNextEdge(current, edges);
            orderedEdges.Add(next);
        }

        return orderedEdges;
    }

    private static Edge GetNextEdge(Edge current, IReadOnlyCollection<Edge> allEdges)
    {
        var withStartEqualEnd = allEdges.FirstOrDefault(x => x.Start == current.End);
        if (withStartEqualEnd != default)
            return withStartEqualEnd;

        var oneAhead = current.End.MoveInDirection(current.Normal.RotateClockwise());
        var withStart1Ahead = allEdges.FirstOrDefault(x => x.Start == oneAhead);
        if (withStart1Ahead != default)
            return withStart1Ahead;
        
        var diagonalLeftAhead = current.End.MoveInDirection(current.Normal).MoveInDirection(current.Normal.RotateClockwise());
        var withStartDiagonalLeft = allEdges.FirstOrDefault(x => x.Start == diagonalLeftAhead);
        if (withStartDiagonalLeft != default)
            return withStartDiagonalLeft;

        throw new Exception("Next edge not found");
    }

    private static void ConnectValidAdjacentEdgesSinglePass(List<Edge> edges, HashSet<Edge> connected, int numRotations, Dictionary<Location, Location> edgeMap)
    {
        for (int i = 0; i < edges.Count; i++)
        {
            var current = edges[i];
            var gap = 2 * numRotations - 1; // 1,3,5,7,...
            var next = edges[(i + gap) % edges.Count];
            
            if (connected.Contains(current) || connected.Contains(next))
                continue;

            if (current.Normal.RotateClockwise(numRotations) == next.Normal.Opposite())
            {
                ConnectEdges(current, next, edgeMap);
                ConnectEdges(next, current, edgeMap);
                connected.Add(current);
                connected.Add(next);
            }
        }
    }
    
    private static void ConnectEdges(Edge current, Edge next, Dictionary<Location, Location> edgeMap)
    {
        var startEdgePositions = current.GetPositions().ToList();
        var endEdgePositions = next.GetPositionsReversed().ToList();

        for (int i = 0; i < startEdgePositions.Count; i++)
        {
            var start = startEdgePositions[i];
            var end = endEdgePositions[i];
            var startLocation = new Location(start, current.Normal);
            var endLocation = new Location(end, next.Normal.Opposite());
            edgeMap[startLocation] = endLocation;
        }
    }
}