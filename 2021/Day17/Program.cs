using System.Numerics;

var input = File.ReadAllLines("input.txt")[0];
var inputSplit = input.Split(new [] {'=', ',', '.'}, StringSplitOptions.RemoveEmptyEntries);

int minX = int.Parse(inputSplit[1]);
int maxX = int.Parse(inputSplit[2]);
int minY = int.Parse(inputSplit[4]);
int maxY = int.Parse(inputSplit[5]);

var targetArea = new TargetArea(minX, maxX, minY, maxY);

var heightsReached = new Dictionary<Vector2, int>();

foreach (var initialVelocity in VelocitiesToTest(targetArea.MaxX, targetArea.MinY, 1000))
{
    var probeState = new ProbeState(Vector2.Zero, initialVelocity);
    int maxHeightReached = 0;
    while (!IsBeyondArea(probeState.Position, targetArea))
    {
        probeState = Advance1Step(probeState);
        if (probeState.Position.Y > maxHeightReached)
            maxHeightReached = (int)probeState.Position.Y;

        if (IsInTargetArea(probeState.Position, targetArea))
        {
            heightsReached[initialVelocity] = maxHeightReached;
            break;
        }
    }
}

int maxOverallHeight = heightsReached.Values.Max();

Console.WriteLine($"Max height reached: {maxOverallHeight}");
Console.WriteLine($"Total number of initial velocities that reach the target: {heightsReached.Count}");


static ProbeState Advance1Step(ProbeState probeState)
{
    var position = probeState.Position + probeState.Velocity;
    var xVelocity = probeState.Velocity.X - Math.Sign(probeState.Velocity.X);
    var yVelocity = probeState.Velocity.Y - 1;
    var velocity = new Vector2(xVelocity, yVelocity);

    return new ProbeState(position, velocity);
}

static bool IsInTargetArea(Vector2 position, TargetArea area)
{
    return position.X >= area.MinX && position.X <= area.MaxX &&
           position.Y >= area.MinY && position.Y <= area.MaxY;
}

static bool IsBeyondArea(Vector2 position, TargetArea area)
{
    return position.X >= area.MaxX || position.Y <= area.MinY;
}

static IEnumerable<Vector2> VelocitiesToTest(int maxX, int minY, int maxY)
{
    for (int x = 0; x <= maxX; x++)
    {
        for (int y = minY; y <= maxY; y++)
        {
            yield return new Vector2(x,y);
        }
    }
}

public record struct ProbeState(Vector2 Position, Vector2 Velocity);

public record struct TargetArea(int MinX, int MaxX, int MinY, int MaxY);