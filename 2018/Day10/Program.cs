using System.Numerics;

var input = File.ReadAllLines("input.txt");

var points = input.Select(GetPositionAndVelocityFromInputLine).ToList();

float previousArea = float.PositiveInfinity;
float area = CalculateArea(points);

int timeStepCount = 0;
while (area < previousArea)
{
    AdvanceTimeSteps(points, 1);
    timeStepCount++;
    previousArea = area;
    area = CalculateArea(points);
}

AdvanceTimeSteps(points, -1);
RenderPoints(points);

Console.WriteLine($"Seconds taken: {timeStepCount - 1}");

static Point GetPositionAndVelocityFromInputLine(string line)
{
    var split = line.Split(new[] { "<", ">", "," }, StringSplitOptions.TrimEntries);
    var position = new Vector2(int.Parse(split[1]), int.Parse(split[2]));
    var velocity = new Vector2(int.Parse(split[4]), int.Parse(split[5]));

    return new Point(position, velocity);
}

static void AdvanceTimeSteps(IList<Point> points, int timeSteps)
{
    for (int i = 0; i < points.Count; i++)
    {
        var point = points[i];
        var newPosition = point.Position + point.Velocity * timeSteps;
        points[i] = point with { Position = newPosition };
    }
}

static float CalculateArea(IReadOnlyCollection<Point> points)
{
    float xSize = points.Max(p => p.Position.X) - points.Min(p => p.Position.X);
    float ySize = points.Max(p => p.Position.Y) - points.Min(p => p.Position.Y);

    return xSize * ySize;
}

static void RenderPoints(List<Point> points)
{
    int minX = (int)points.Min(p => p.Position.X);
    int maxX = (int)points.Max(p => p.Position.X);
    int minY = (int)points.Min(p => p.Position.Y);
    int maxY = (int)points.Max(p => p.Position.Y);
    
    for (int y = minY; y <= maxY; y++)
    {
        for (int x = minX; x <= maxX; x++)
        {
            var position = new Vector2(x, y);
            Console.Write(points.Any(p => p.Position == position) ? '#' : '.');
        }

        Console.WriteLine();
    }
}


public record struct Point(Vector2 Position, Vector2 Velocity);