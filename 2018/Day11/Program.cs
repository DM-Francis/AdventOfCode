using System.Diagnostics;

var serialNumber = 9306;

var grid = new int[300,300];

for (int x = 0; x < grid.GetLength(0); x++)
{
    for (int y = 0; y < grid.GetLength(1); y++)
    {
        grid[x, y] = CalculatePowerLevel(x + 1, y + 1, serialNumber);
    }
}

// Part 1
var topLeftCoordFor3X3WithMostPower = FindSquareWithLargestTotalPower(grid, 3);
Console.WriteLine($"The 3x3 section with the most power has top left coord (and power): {topLeftCoordFor3X3WithMostPower}");

// Part 2
int maxPower = int.MinValue;
(int XCoord, int YCoord, int SquareSize) bestSquare = default;
var stopwatch = new Stopwatch();

for (int squareSize = 1; squareSize <= 300; squareSize++)
{
    Console.WriteLine($"Starting square size = {squareSize}...  ({stopwatch.ElapsedMilliseconds}ms)");
    stopwatch.Restart();
    var result = FindSquareWithLargestTotalPower(grid, squareSize);
    if (result.TotalPower <= maxPower)
        continue;

    maxPower = result.TotalPower;
    bestSquare = (result.XCoord, result.YCoord, squareSize);
}

Console.WriteLine($"The section with the most total power has coordinate and power: {bestSquare}, {maxPower}");

static int CalculatePowerLevel(int xCoord, int yCoord, int serialNumber)
{
    int rackId = xCoord + 10;
    int powerLevel = rackId * yCoord;
    powerLevel += serialNumber;
    powerLevel *= rackId;
    powerLevel = (powerLevel / 100) % 10;
    return powerLevel - 5;
}

static (int XCoord, int YCoord, int TotalPower) FindSquareWithLargestTotalPower(int[,] grid, int squareSize)
{
    int maxPower = int.MinValue;
    (int XCoord, int YCoord) coordOfMaxPower = default;

    for (int x = 0; x < grid.GetLength(0) - squareSize; x++)
    {
        for (int y = 0; y < grid.GetLength(1) - squareSize; y++)
        {
            int totalPower = GetTotalPowerForSquareWithTopLeftIndices(x, y, grid, squareSize);
            if (totalPower <= maxPower)
                continue;
            
            maxPower = totalPower;
            coordOfMaxPower = (x + 1, y + 1);
        }
    }

    return (coordOfMaxPower.XCoord, coordOfMaxPower.YCoord, maxPower);
}

static int GetTotalPowerForSquareWithTopLeftIndices(int xTopLeft, int yTopLeft, int[,] grid, int squareSize)
{
    int totalPower = 0;
    for (int x = xTopLeft; x < xTopLeft + squareSize; x++)
    {
        for (int y = yTopLeft; y < yTopLeft + squareSize; y++)
        {
            totalPower += grid[x, y];
        }
    }
    
    return totalPower;
}