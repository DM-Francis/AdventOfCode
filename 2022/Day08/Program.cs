var input = File.ReadAllLines("input.txt");

var grid = GetGridFromInput(input);

// Part 1
int visibleCount = CountVisibleTrees(grid);
Console.WriteLine($"Visible trees: {visibleCount}");

// Part 2
int maxScore = GetMaxScenicScore(grid);
Console.WriteLine($"Max scenic score: {maxScore}");

static int[,] GetGridFromInput(IReadOnlyList<string> input)
{
    int rowCount = input.Count;
    int colCount = input[0].Length;
    
    int[,] ints = new int[rowCount, colCount];
    for (int row = 0; row < rowCount; row++)
    {
        for (int col = 0; col < colCount; col++)
        {
            ints[row, col] = int.Parse(input[row][col].ToString());
        }
    }

    return ints;
}

static int CountVisibleTrees(int[,] grid)
{
    int maxRow = grid.GetUpperBound(0);
    int maxCol = grid.GetUpperBound(1);

    int visibleCount = 0;
    
    for (int row = 0; row <= maxRow; row++)
    {
        for (int col = 0; col <= maxCol; col++)
        {
            if (TreeIsVisible(row, col, grid))
                visibleCount++;
        }
    }

    return visibleCount;
}

static int GetMaxScenicScore(int[,] grid)
{
    int maxRow = grid.GetUpperBound(0);
    int maxCol = grid.GetUpperBound(1);

    int maxScenicScore = 0;
    
    for (int row = 0; row <= maxRow; row++)
    {
        for (int col = 0; col <= maxCol; col++)
        {
            int score = GetScenicScoreForTree(row, col, grid);
            if (score > maxScenicScore)
                maxScenicScore = score;
        }
    }

    return maxScenicScore;
}

static bool TreeIsVisible(int row, int col, int[,] grid)
{
    int maxRow = grid.GetUpperBound(0);
    int maxCol = grid.GetUpperBound(1);

    if (row == 0 || row == maxRow || col == 0 || col == maxCol)
        return true;

    return IsVisibleFromLeft(row, col, grid)
           || IsVisibleFromRight(row, col, grid)
           || IsVisibleFromAbove(row, col, grid)
           || IsVisibleFromBelow(row, col, grid);
}

static bool IsVisibleFromLeft(int row, int col, int[,] grid)
{
    int height = grid[row, col];
    for (int i = col - 1; i >= 0; i--)
    {
        if (grid[row, i] >= height)
            return false;
    }

    return true;
}

static bool IsVisibleFromRight(int row, int col, int[,] grid)
{
    int height = grid[row, col];
    int maxCol = grid.GetUpperBound(1);
    
    for (int i = col + 1; i <= maxCol; i++)
    {
        if (grid[row, i] >= height)
            return false;
    }

    return true;
}

static bool IsVisibleFromAbove(int row, int col, int[,] grid)
{
    int height = grid[row, col];
    for (int i = row - 1; i >= 0; i--)
    {
        if (grid[i, col] >= height)
            return false;
    }

    return true;
}

static bool IsVisibleFromBelow(int row, int col, int[,] grid)
{
    int height = grid[row, col];
    int maxRow = grid.GetUpperBound(0);
    
    for (int i = row + 1; i <= maxRow; i++)
    {
        if (grid[i, col] >= height)
            return false;
    }

    return true;
}

static int GetScenicScoreForTree(int row, int col, int[,] grid)
{
    int maxRow = grid.GetUpperBound(0);
    int maxCol = grid.GetUpperBound(1);

    if (row == 0 || row == maxRow || col == 0 || col == maxCol)
        return 0;

    return TreesVisibleToLeft(row, col, grid)
           * TreesVisibleToRight(row, col, grid)
           * TreesVisibleAbove(row, col, grid)
           * TreesVisibleBelow(row, col, grid);
}

static int TreesVisibleToLeft(int row, int col, int[,] grid)
{
    int height = grid[row, col];
    int count = 0;
    for (int c = col - 1; c >= 0; c--)
    {
        count++;
        if (grid[row, c] >= height)
            return count;
    }

    return count;
}

static int TreesVisibleToRight(int row, int col, int[,] grid)
{
    int height = grid[row, col];
    int maxCol = grid.GetUpperBound(1);
    int count = 0;
    for (int c = col + 1; c <= maxCol; c++)
    {
        count++;
        if (grid[row, c] >= height)
            return count;
    }

    return count;
}

static int TreesVisibleAbove(int row, int col, int[,] grid)
{
    int height = grid[row, col];
    int count = 0;
    for (int r = row - 1; r >= 0; r--)
    {
        count++;
        if (grid[r, col] >= height)
            return count;
    }

    return count;
}

static int TreesVisibleBelow(int row, int col, int[,] grid)
{
    int height = grid[row, col];
    int maxRow = grid.GetUpperBound(0);
    int count = 0;
    for (int r = row + 1; r <= maxRow; r++)
    {
        count++;
        if (grid[r, col] >= height)
            return count;
    }

    return count;
}