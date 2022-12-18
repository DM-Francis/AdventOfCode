var input = File.ReadAllLines("input.txt");
var inputPoints = input.Select(x =>
{
    var values = x.Split(',').Select(int.Parse).ToList();
    return new Point(values[0], values[1], values[2]);
}).ToList();


// Part 1
var pointsInDroplet = new HashSet<Point>();
int totalExposedSides = 0;

foreach (var point in inputPoints)
{
    pointsInDroplet.Add(point);
    int touchingAdjacentPoints = GetAdjacentPoints(point).Count(adj => pointsInDroplet.Contains(adj));

    totalExposedSides += 6 - 2 * touchingAdjacentPoints;
}

Console.WriteLine($"Total surface area: {totalExposedSides}");

// Part 2
var externalPoints = new Dictionary<Point, int>();  // Points not in the droplet, but within the enclosing box, and how many points in the droplet it is adjacent to.

int minX = inputPoints.Min(p => p.X);
int maxX = inputPoints.Max(p => p.X);
int minY = inputPoints.Min(p => p.Y);
int maxY = inputPoints.Max(p => p.Y);
int minZ = inputPoints.Min(p => p.Z);
int maxZ = inputPoints.Max(p => p.Z);

// We'll do a flood fill algorithm starting from the bottom corner of the droplet i.e. at (minX - 1, minY - 1, minZ - 1)
// We'll only flood those points which are within a box enclosing the droplet, and for each, we'll count how many surfaces on the droplet it is adjacent to

var box = new Box(minX - 1, maxX + 1, minY - 1, maxY + 1, minZ - 1, maxZ + 1);
var queue = new HashSet<Point> { new(minX - 1, minY - 1, minZ - 1) };

long iterations = 0;
while (queue.Count > 0)
{
    iterations++;
    var current = queue.First();
    queue.Remove(current);

    var adjacentPoints = GetAdjacentPoints(current).ToList();
    
    int adjacentToDropletCount = adjacentPoints.Count(p => pointsInDroplet.Contains(p));
    externalPoints[current] = adjacentToDropletCount;
    
    foreach (var next in adjacentPoints.Where(p => IsInBox(p, box)
                                                   && !externalPoints.ContainsKey(p)
                                                   && !pointsInDroplet.Contains(p)))
    {
        queue.Add(next);
    }
}

int totalExteriorSurfaceArea = externalPoints.Values.Sum();

Console.WriteLine($"Total exterior surface area: {totalExteriorSurfaceArea}");
Console.WriteLine($"Total iterations: {iterations}");

static IEnumerable<Point> GetAdjacentPoints(Point point)
{
    yield return point with { X = point.X + 1 };
    yield return point with { X = point.X - 1 };
    yield return point with { Y = point.Y + 1 };
    yield return point with { Y = point.Y - 1 };
    yield return point with { Z = point.Z + 1 };
    yield return point with { Z = point.Z - 1 };
}

static bool PointsAreAdjacent(Point a, Point b)
{
    return Math.Abs(a.X - b.X) == 1 && a.Y == b.Y && a.Z == b.Z ||
           Math.Abs(a.Y - b.Y) == 1 && a.X == b.X && a.Z == b.Z ||
           Math.Abs(a.Z - b.Z) == 1 && a.X == b.X && a.Y == b.Y;
}

static bool IsInBox(Point point, Box box)
{
    return point.X >= box.MinX && point.X <= box.MaxX &&
           point.Y >= box.MinY && point.Y <= box.MaxY &&
           point.Z >= box.MinZ && point.Z <= box.MaxZ;
}

public record struct Point(int X, int Y, int Z);
public record struct Box(int MinX, int MaxX, int MinY, int MaxY, int MinZ, int MaxZ);