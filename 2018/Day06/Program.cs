var input = File.ReadAllLines("input.txt");

var coords = new List<(int Id, int X, int Y)>(input.Length);
for (int i = 0; i < input.Length; i++)
{
    var split = input[i].Split(", ");
    int x = int.Parse(split[0]);
    int y = int.Parse(split[1]);
    coords.Add((i, x, y));
}

int maxX = coords.Max(c => c.X);
int maxY = coords.Max(c => c.Y);


// Part 1
var grid = new int?[maxX + 1, maxY + 1];

for (int x = 0; x <= grid.GetUpperBound(0); x++)
{
    for (int y = 0; y <= grid.GetUpperBound(1); y++)
    {
        var closest2Coords = coords
            .Select(c => new {Coord = c, Distance = ManhattanDistance((x, y), (c.X, c.Y))})
            .OrderBy(v => v.Distance)
            .Take(2)
            .ToList();

        if (closest2Coords[0].Distance < closest2Coords[1].Distance)
            grid[x, y] = closest2Coords[0].Coord.Id;
        // Otherwise multiple coords are just as close, so leave as null
    }
}

// Get counts per Id
var countsPerId = new Dictionary<int, int>();

foreach (var location in grid)
{
    if (location is not null)
    {
        if (countsPerId.TryGetValue(location.Value, out int currentCount))
            countsPerId[location.Value] = currentCount + 1;
        else
            countsPerId[location.Value] = 1;
    }
}

// Get ids on the edge - these will be the ids with infinite area, so exclude them
var idsToExclude = GetIdsToExclude(grid);

int largestArea = countsPerId.Where(kv => !idsToExclude.Contains(kv.Key)).Max(kv => kv.Value);

Console.WriteLine($"Largest non-infinite area: {largestArea}");


// Part 2
var regionGrid = new bool[maxX + 1, maxY + 1];
for (int x = 0; x <= regionGrid.GetUpperBound(0); x++)
{
    for (int y = 0; y <= regionGrid.GetUpperBound(1); y++)
    {
        bool isInRegion = coords.Sum(c => ManhattanDistance((x, y), (c.X, c.Y))) < 10000;
        regionGrid[x, y] = isInRegion;
    }
}

int sizeOfRegion = 0;
foreach (bool value in regionGrid)
{
    if (value) sizeOfRegion++;
}

Console.WriteLine($"Size of safe region: {sizeOfRegion}");



static int ManhattanDistance((int X, int Y) first, (int X, int Y) second)
{
    return Math.Abs(first.X - second.X) + Math.Abs(first.Y - second.Y);
}

static HashSet<int> GetIdsToExclude(int?[,] grid)
{
    int maxX = grid.GetUpperBound(0);
    int maxY = grid.GetUpperBound(1);
    
    var hashSet = new HashSet<int>();
    for (int x = 0; x <= maxX; x++)
    {
        UpdateIdsToExclude(grid[x, 0]);
    }
    for (int x = 0; x <= maxX; x++)
    {
        UpdateIdsToExclude(grid[x, maxY]);
    }
    for (int y = 0; y <= maxY; y++)
    {
        UpdateIdsToExclude(grid[0, y]);
    }
    for (int y = 0; y <= maxY; y++)
    {
        UpdateIdsToExclude(grid[maxX, y]);
    }

    return hashSet;

    void UpdateIdsToExclude(int? value)
    {
        if (value is null)
            return;

        hashSet.Add(value.Value);
    }
}