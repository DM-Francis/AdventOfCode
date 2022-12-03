var data = File.ReadAllLines("input.txt");
var testData = new string[]
{
    "6,10",
    "0,14",
    "9,10",
    "0,3",
    "10,4",
    "4,11",
    "6,0",
    "6,12",
    "4,1",
    "0,13",
    "10,12",
    "3,4",
    "3,0",
    "8,4",
    "1,10",
    "2,14",
    "8,10",
    "9,0",
    "",
    "fold along y=7",
    "fold along x=5"
};

string[] pointsData = data.Where(line => !string.IsNullOrEmpty(line) && line[0] != 'f').ToArray();
string[] foldsData = data.Where(line => !string.IsNullOrEmpty(line) && line[0] == 'f').ToArray();

var grid = GetGridFromPointsData(pointsData);

// Part 1
var firstFoldGrid = ApplyFoldToGrid(foldsData[0], grid);
Console.WriteLine($"Visible dots after first fold: {firstFoldGrid.Count}");

// Part 2
foreach (string fold in foldsData)
{
    grid = ApplyFoldToGrid(fold, grid);
}

RenderGrid(grid);



static HashSet<(int X, int Y)> GetGridFromPointsData(string[] pointsData)
{
    var pointsGrid = new HashSet<(int X, int Y)>();
    foreach (string pointString in pointsData)
    {
        var pointParts = pointString.Split(',');
        var point = (int.Parse(pointParts[0]), int.Parse(pointParts[1]));

        pointsGrid.Add(point);
    }

    return pointsGrid;
}

static HashSet<(int X, int Y)> ApplyFoldToGrid(string foldString, HashSet<(int X, int Y)> grid)
{
    var fold = foldString.Split(' ')[2];
    var foldParts = fold.Split('=');

    string axis = foldParts[0];
    int value = int.Parse(foldParts[1]);

    var newGrid = new HashSet<(int X, int Y)>();

    foreach(var (X, Y) in grid)
    {
        if (axis == "x" && X > value)
        {
            var newPoint = (2 * value - X, Y);
            newGrid.Add(newPoint);
        }
        else if (axis == "y" && Y > value)
        {
            var newPoint = (X, 2 * value - Y);
            newGrid.Add(newPoint);
        }
        else
        {
            newGrid.Add((X, Y));
        }
    }

    return newGrid;
}


static void RenderGrid(HashSet<(int X, int Y)> grid)
{
    int maxX = grid.Max(p => p.X);
    int maxY = grid.Max(p => p.Y);

    for (int y = 0; y <= maxY; y++)
    {
        for (int x = 0; x <= maxX; x++)
        {
            if (grid.Contains((x, y)))
                Console.Write('█');
            else
                Console.Write(' ');
        }

        Console.WriteLine();
    }
}