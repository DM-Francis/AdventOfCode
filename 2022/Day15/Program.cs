var input = File.ReadAllLines("input.txt");
// var input = """
// Sensor at x=2, y=18: closest beacon is at x=-2, y=15
// Sensor at x=9, y=16: closest beacon is at x=10, y=16
// Sensor at x=13, y=2: closest beacon is at x=15, y=3
// Sensor at x=12, y=14: closest beacon is at x=10, y=16
// Sensor at x=10, y=20: closest beacon is at x=10, y=16
// Sensor at x=14, y=17: closest beacon is at x=10, y=16
// Sensor at x=8, y=7: closest beacon is at x=2, y=10
// Sensor at x=2, y=0: closest beacon is at x=2, y=10
// Sensor at x=0, y=11: closest beacon is at x=2, y=10
// Sensor at x=20, y=14: closest beacon is at x=25, y=17
// Sensor at x=17, y=20: closest beacon is at x=21, y=22
// Sensor at x=16, y=7: closest beacon is at x=15, y=3
// Sensor at x=14, y=3: closest beacon is at x=15, y=3
// Sensor at x=20, y=1: closest beacon is at x=15, y=3
// """.Split('\n');

var sensorsAndBeacons = new List<(Position Sensor, Position Beacon)>();
foreach (var line in input)
{
    var split = line.Split(new[] { ' ', ',', '=', ':' }, StringSplitOptions.RemoveEmptyEntries);
    var sensor = new Position(int.Parse(split[3]), int.Parse(split[5]));
    var beacon = new Position(int.Parse(split[11]), int.Parse(split[13]));
    
    sensorsAndBeacons.Add((sensor, beacon));
}

int rowY = 2000000;
var coveragesOnRow = new List<(int Min, int Max)>();

foreach (var (sensor, beacon) in sensorsAndBeacons)
{
    int distanceToBeacon = sensor.DistanceTo(beacon);
    int distanceToRow = Math.Abs(sensor.Y - rowY);

    if (distanceToRow > distanceToBeacon)
        continue;

    int minXOnRow = sensor.X - (distanceToBeacon - distanceToRow);
    int maxXOnRow = sensor.X + (distanceToBeacon - distanceToRow);
    
    coveragesOnRow.Add((minXOnRow, maxXOnRow));
}

var spotsCovered = new HashSet<int>();
foreach (var coverage in coveragesOnRow)
{
    for (int x = coverage.Min; x <= coverage.Max; x++)
    {
        spotsCovered.Add(x);
    }
}

foreach (var beacon in sensorsAndBeacons.Select(x => x.Beacon).Where(b => b.Y == rowY))
{
    spotsCovered.Remove(beacon.X);
}

int totalSpotsCovered = spotsCovered.Count;
Console.WriteLine($"Total spots covered on row {rowY}: {totalSpotsCovered}");

// Part 2
var sensorRanges = sensorsAndBeacons.ToDictionary(sb => sb.Sensor, sb => sb.Sensor.DistanceTo(sb.Beacon));

var beaconPosition = GetOnlyAvailableDistressBeaconPosition(sensorRanges);
long tuningFrequency = (long)beaconPosition.X * 4000000 + beaconPosition.Y;

Console.WriteLine($"Beacon found at position {beaconPosition} with tuning frequency {tuningFrequency}");

static Position GetOnlyAvailableDistressBeaconPosition(Dictionary<Position, int> sensorRanges)
{
    var validPositions = new List<Position>();
    foreach (var (sensor, range) in sensorRanges)
    {
        var borderingPositions = GetBorderingPositionsFor(sensor, range);
        foreach (var position in borderingPositions.Where(p => p.X is >= 0 and <= 4000000 && p.Y is >= 0 and <= 4000000))
        {
            if (!sensorRanges.Any(sr => position.DistanceTo(sr.Key) <= sr.Value))
                return position;
        }
    }

    throw new InvalidOperationException("No valid distress beacon positions found");
}

static IEnumerable<Position> GetBorderingPositionsFor(Position sensor, int range)
{
    foreach (var relativePosition in GetAllRelativePositionsAtDistance(range + 1))
    {
        yield return sensor + relativePosition;
    }
}

static IEnumerable<Position> GetAllRelativePositionsAtDistance(int range)
{
    yield return new Position(range, 0);
    yield return new Position(0, range);
    yield return new Position(-range, 0);
    yield return new Position(0, -range);

    for (int i = 1; i < range; i++)
    {
        int x = range - i;
        int y = i;
        
        yield return new Position(x , y);
        yield return new Position(x, -y);
        yield return new Position(-x, y);
        yield return new Position(-x, -y);
    }
}


public readonly record struct Position(int X, int Y)
{
    public int DistanceTo(Position other)
    {
        return Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
    }
    
    public static Position operator +(Position a, Position b)
    {
        return new Position(a.X + b.X, a.Y + b.Y);
    }
}