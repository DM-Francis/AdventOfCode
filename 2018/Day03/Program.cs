﻿
using Day03;

var input = File.ReadAllLines("input.txt");
var claims = input.Select(Claim.FromString).ToList();

var grid = new Dictionary<(int X, int Y), List<int>>();


foreach (var claim in claims)
{
    AddClaimToGrid(grid, claim);
}

// Part 1
int overlapCount = grid.Values.Count(ids => ids.Count > 1);

Console.WriteLine($"Overlap count: {overlapCount}");

// Part 2
var overlapsPerId = new Dictionary<int, int>(claims.Select(c => new KeyValuePair<int, int>(c.Id, 0)));

foreach (var squareIds in grid.Values)
{
    if (squareIds.Count == 1)
        continue;
    
    foreach (int id in squareIds)
    {
        overlapsPerId[id] += 1;
    }
}

int idWithNoOverlaps = overlapsPerId.Single(kv => kv.Value == 0).Key;

Console.WriteLine($"Id with no overlaps: {idWithNoOverlaps}");
RenderGrid(grid);  // Doesnt really work on console

// Extra
var maxOverlapCount = grid.Values.Max(x => x.Count);
Console.WriteLine($"Max overlap count: {maxOverlapCount}");

static void AddClaimToGrid(IDictionary<(int X, int Y), List<int>> grid, Claim claim)
{
    for (int x = claim.FromLeft; x < claim.FromLeft + claim.Width; x++)
    {
        for (int y = claim.FromTop; y < claim.FromTop + claim.Height; y++)
        {
            if (grid.TryGetValue((x, y), out List<int>? currentIds))
                currentIds.Add(claim.Id);
            else
                grid[(x, y)] = new List<int> {claim.Id};
        }
    }
}

static void RenderGrid(IDictionary<(int X, int Y), List<int>> grid)
{
    int maxX = grid.Keys.Max(k => k.X);
    int maxY = grid.Keys.Max(k => k.Y);

    for (int y = 0; y <= maxY; y++)
    {
        for (int x = 0; x <= maxX; x++)
        {
            if (grid.TryGetValue((x, y), out List<int>? ids))
                Console.Write(ids.Count);
            else
                Console.Write('.');
        }
        
        Console.WriteLine();
    }
}