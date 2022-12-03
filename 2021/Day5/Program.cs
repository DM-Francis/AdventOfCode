using Day5;

var input = File.ReadAllLines("input.txt");

var pointCounts = new Dictionary<Point, int>();

foreach(string line in input)
{
    var points = GetPointsForLineInput(line).ToList();

    foreach(Point point in points)
    {
        if (pointCounts.ContainsKey(point))
            pointCounts[point]++;
        else
            pointCounts[point] = 1;
    }
}

int overlapCount = pointCounts.Values.Count(v => v > 1);

Console.WriteLine($"Overlap count: {overlapCount}");
Console.WriteLine($"Max overlap: {pointCounts.Values.Max()}");

static IEnumerable<Point> GetPointsForLineInput(string lineString)
{
    var lineParts = lineString.Split(' ');
    var startingPoint = Point.FromString(lineParts[0]);
    var endingPoint = Point.FromString(lineParts[2]);

    var line = new Line(startingPoint, endingPoint);

    return line.GetAllPointsOnLine();
}