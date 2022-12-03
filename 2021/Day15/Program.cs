var testInput = new string[]
{
    "1163751742",
    "1381373672",
    "2136511328",
    "3694931569",
    "7463417111",
    "1319128137",
    "1359912421",
    "3125421639",
    "1293138521",
    "2311944581"
};

var input = File.ReadAllLines("input.txt");

var grid = GetGridFromInput(input);
var fullGrid = GetFullGridFromStarterGrid(grid);

var pathRisks = new int?[fullGrid.GetLength(0), fullGrid.GetLength(1)];

int updates;
int iterations = 0;
do
{
    updates = UpdatePathRisks(fullGrid, pathRisks);
    iterations++;
}
while (updates > 0);

int lowestTotalRisk = pathRisks[0, 0]!.Value;

Console.WriteLine($"Lowest total risk: {lowestTotalRisk}");
Console.WriteLine($"Total iterations: {iterations}");

static int UpdatePathRisks(int[,] grid, int?[,] pathRisks)
{
    int rowMax = grid.GetLength(0) - 1;
    int colMax = grid.GetLength(1) - 1;

    pathRisks[rowMax, colMax] = 0;

    int updates = 0;
    // Iterate from bottom right
    for (int row = rowMax; row >= 0; row--)
    {
        for (int col = colMax; col >= 0; col--)
        {
            foreach(var adj in GetAdjacentCoordinates(row, col, rowMax, colMax))
            {
                int? adjValue = pathRisks[adj.Row, adj.Col];
                if (adjValue is null)
                    continue;
                
                int adjRisk = adjValue.Value + grid[adj.Row, adj.Col];

                if (pathRisks[row, col] is null || adjRisk < pathRisks[row, col])
                {
                    pathRisks[row, col] = adjRisk;
                    updates++;
                }
            }
        }
    }

    return updates;
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

static int[,] GetGridFromInput(string[] input)
{
    var grid = new int[input.Length, input[0].Length];

    for (int row = 0; row < input.Length; row++)
    {
        for (int col = 0; col < input[0].Length; col++)
        {
            grid[row, col] = int.Parse(input[row][col].ToString());
        }
    }

    return grid;
}


static int[,] GetFullGridFromStarterGrid(int[,] grid)
{
    var fullGrid = new int[grid.GetLength(0) * 5, grid.GetLength(1) * 5];

    int rowLength = grid.GetLength(0);
    int colLength = grid.GetLength(1);

    for (int tileRow = 0; tileRow < 5; tileRow++)
    {
        for (int tileCol = 0; tileCol < 5; tileCol++)
        {
            for (int row = 0; row < rowLength; row++)
            {
                for (int col = 0; col < colLength; col++)
                {
                    int newVal = (grid[row, col] + tileRow + tileCol - 1) % 9 + 1;

                    fullGrid[tileRow * rowLength + row, tileCol * colLength + col] = newVal;
                }
            }
        }
    }

    return fullGrid;
}