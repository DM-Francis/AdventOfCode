using System.Numerics;
using Day14;

var input = File.ReadAllText("input.txt");
// var input = """
// 498,4 -> 498,6 -> 496,6
// 503,4 -> 502,4 -> 502,9 -> 494,9
// """;

// Part 1
var cave = new Cave(input);

cave.Render();

AdvanceTimeUntilCaveIsFull(cave);

cave.Render();
int sandCount = cave.GetSandCount();

Console.WriteLine($"Total sand units before falling to abyss: {sandCount}");

// Part 2
var cave2 = new Cave(input, true);
cave2.Render();

AdvanceTimeUntilCaveIsFull(cave2);
int sandCount2 = cave2.GetSandCount();

Console.WriteLine($"Total sand units before blocking up source: {sandCount2}");
cave2.Render();

static void AdvanceTimeUntilCaveIsFull(Cave cave)
{
    var sandSource = new Vector2(500, 0);
    var currentSandUnitPosition = sandSource;
    while (!cave.IsFull)
    {
        currentSandUnitPosition = AdvanceOneTimeStep(cave, currentSandUnitPosition, sandSource);
    }
}

static Vector2 AdvanceOneTimeStep(Cave cave, Vector2 currentSandUnitPosition, Vector2 sandSource)
{
    cave[currentSandUnitPosition] = Square.Empty;
    var below = currentSandUnitPosition + new Vector2(0, 1);
    var belowLeft = currentSandUnitPosition + new Vector2(-1, 1);
    var belowRight = currentSandUnitPosition + new Vector2(1, 1);

    if (below.Y > cave.MaxY) // Into the abyss
    {
        cave.IsFull = true;
        cave[currentSandUnitPosition] = Square.Empty;
        return Vector2.Zero;
    }

    if (cave[below] == Square.Empty)
    {
        cave[below] = Square.Sand;
        return below;
    }

    if (cave[belowLeft] == Square.Empty)
    {
        cave[belowLeft] = Square.Sand;
        return belowLeft;
    }

    if (cave[belowRight] == Square.Empty)
    {
        cave[belowRight] = Square.Sand;
        return belowRight;
    }

    cave[currentSandUnitPosition] = Square.Sand;
    if (currentSandUnitPosition == sandSource) // Source blocked
        cave.IsFull = true;
    
    return sandSource;
}





public enum Square
{
    Empty,
    Rock,
    Sand
}