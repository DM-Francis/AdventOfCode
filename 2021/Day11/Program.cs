var data = File.ReadAllLines("input.txt");

var testData = new string[]
{
    "5483143223",
    "2745854711",
    "5264556173",
    "6141336146",
    "6357385478",
    "4167524645",
    "2176841721",
    "6882881134",
    "4846848554",
    "5283751526"
};


int[,] grid = GetGridFromInput(data);

int totalFlashes = 0;

Console.WriteLine("Before any steps:");
RenderGrid(grid);
for (int i = 0; i < 100; i++)
{
    totalFlashes += ApplyStepToGrid(grid);

    Console.WriteLine($"\nAfter step {i + 1}:");
    RenderGrid(grid);
}

Console.WriteLine($"Total flashes after 100 steps: {totalFlashes}");


int[,] grid2 = GetGridFromInput(data);
int flashes;
int steps = 0;
do
{
    flashes = ApplyStepToGrid(grid2);
    steps++;
}
while (flashes != 100);

Console.WriteLine($"First step where all Octopuses flash: {steps}");

static int[,] GetGridFromInput(string[] input)
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

static int ApplyStepToGrid(int[,] grid)
{
    int rowLength = grid.GetLength(0);
    int colLength = grid.GetLength(1);

    bool[,] flashed = new bool[rowLength, colLength];

    // Increase all energy levels by 1
    for (int row = 0; row < rowLength; row++)
    {
        for (int col = 0; col < colLength; col++)
        {
            grid[row, col]++;
        }
    }

    int totalFlashCount = 0;
    int flashCount;
    do
    {
        flashCount = ProcessFlashes(grid, flashed);
        totalFlashCount += flashCount;
    }
    while (flashCount > 0);


    // Set flashed octopuses to 0
    for (int row = 0; row < rowLength; row++)
    {
        for (int col = 0; col < colLength; col++)
        {
            if (flashed[row, col])
                grid[row, col] = 0;
        }
    }

    return totalFlashCount;
}

static int ProcessFlashes(int[,] grid, bool[,] flashed)
{
    int rowLength = grid.GetLength(0);
    int colLength = grid.GetLength(1);
    int rowMax = rowLength - 1;
    int colMax = colLength - 1;

    int flashCount = 0;

    for (int row = 0; row < rowLength; row++)
    {
        for (int col = 0; col < colLength; col++)
        {
            if (grid[row, col] > 9 && !flashed[row, col])
            {
                foreach(var coord in GetAdjacentCoordinates(row, col, rowMax, colMax))
                {
                    grid[coord.Row, coord.Col]++;
                }

                flashed[row, col] = true;
                flashCount++;
            }
        }
    }

    return flashCount;
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

    if (row != 0 && col != 0)
        yield return (row - 1, col - 1);

    if (row != 0 && col != colMax)
        yield return (row - 1, col + 1);

    if (row != rowMax && col != 0)
        yield return (row + 1, col - 1);

    if (row != rowMax && col != colMax)
        yield return (row + 1, col + 1);
}


static void RenderGrid(int[,] grid)
{
    Console.ResetColor();
    for (int row = 0; row < grid.GetLength(0); row++)
    {
        for (int col = 0; col < grid.GetLength(1); col++)
        {
            if (grid[row,col] == 0)
                Console.ForegroundColor = ConsoleColor.Green;

            Console.Write(grid[row, col]);
            Console.ResetColor();
        }

        Console.WriteLine();
    }
}