var input = File.ReadAllLines("input.txt");

var testInput = new string[]
{
    "2199943210",
    "3987894921",
    "9856789892",
    "8767896789",
    "9899965678"
};

int[,] map = GetMapFromInput(input);

int riskSum = GetLowPointValues(map).Select(v => v + 1).Sum();
Console.WriteLine($"Total risk: {riskSum}");

var lowPoints = GetLowPointCoordinates(map);

var basins = new List<List<(int Row, int Col)>>();
foreach(var lowPoint in lowPoints)
{
    var basin = GetBasinForPoint(lowPoint.Row, lowPoint.Col, map).ToList();
    basins.Add(basin);
}

var orderedBasins = basins.OrderByDescending(b => b.Count).ToList();
int largestBasinsMultiple = orderedBasins[0].Count * orderedBasins[1].Count * orderedBasins[2].Count;

Console.WriteLine($"Largest basins multiple: {largestBasinsMultiple}");


static int[,] GetMapFromInput(string[] input)
{
    var map = new int[input.Length, input[0].Length];

    for (int row = 0; row < input.Length; row++)
    {
        for (int col = 0; col < input[0].Length; col++)
        {
            map[row, col] = int.Parse(input[row][col].ToString());
        }
    }

    return map;
}


static IEnumerable<int> GetLowPointValues(int[,] map)
{
    return GetLowPointCoordinates(map).Select(coord => map[coord.Row, coord.Col]);
}

static IEnumerable<(int Row, int Col)> GetBasinForPoint(int row, int col, int[,] map)
{
    int value = map[row, col];

    if (value == 9)
        return Enumerable.Empty<(int Row, int Col)>();

    int rowLength = map.GetLength(0);
    int colLength = map.GetLength(1);
    int rowMax = rowLength - 1;
    int colMax = colLength - 1;

    var adjacentCoords = GetAdjacentCoordinates(row, col, rowMax, colMax);

    var basinPoints = new HashSet<(int Row, int Col)>
    {
        (row, col)
    };

    foreach (var coord in adjacentCoords)
    {
        int coordValue = map[coord.Row, coord.Col];
        if (coordValue != 9 && coordValue > value)
        {
            foreach (var basinCoord in GetBasinForPoint(coord.Row, coord.Col, map))
            {
                basinPoints.Add(basinCoord);
            }
        }
    }

    return basinPoints;
}


static IEnumerable<(int Row, int Col)> GetLowPointCoordinates(int[,] map)
{
    int rowLength = map.GetLength(0);
    int colLength = map.GetLength(1);
    int rowMax = rowLength - 1;
    int colMax = colLength - 1;

    for (int row = 0; row < rowLength; row++)
    {
        for (int col = 0; col < colLength; col++)
        {
            int val = map[row, col];
            var adjacentValues = GetAdjacentCoordinates(row, col, rowMax, colMax).Select(coord => map[coord.Row, coord.Col]);

            if (adjacentValues.All(v => v > val))
                yield return (row, col);
        }
    }
}

static IEnumerable<(int Row, int Col)> GetAdjacentCoordinates(int row, int col, int rowMax, int colMax)
{
    if (row != 0)
        yield return (row - 1, col);

    if (col != 0)
        yield return (row, col - 1);

    if (row != rowMax)
        yield return (row + 1, col);

    if (col != colMax)
        yield return (row, col + 1);
}