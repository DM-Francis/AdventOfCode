using Day20;

var input = File.ReadAllText("input");
var exampleInput = """
                   Tile 2311:
                   ..##.#..#.
                   ##..#.....
                   #...##..#.
                   ####.#...#
                   ##.##.###.
                   ##...#.###
                   .#.#.#..##
                   ..#....#..
                   ###...#.#.
                   ..###..###

                   Tile 1951:
                   #.##...##.
                   #.####...#
                   .....#..##
                   #...######
                   .##.#....#
                   .###.#####
                   ###.##.##.
                   .###....#.
                   ..#.#..#.#
                   #...##.#..

                   Tile 1171:
                   ####...##.
                   #..##.#..#
                   ##.#..#.#.
                   .###.####.
                   ..###.####
                   .##....##.
                   .#...####.
                   #.##.####.
                   ####..#...
                   .....##...

                   Tile 1427:
                   ###.##.#..
                   .#..#.##..
                   .#.##.#..#
                   #.#.#.##.#
                   ....#...##
                   ...##..##.
                   ...#.#####
                   .#.####.#.
                   ..#..###.#
                   ..##.#..#.

                   Tile 1489:
                   ##.#.#....
                   ..##...#..
                   .##..##...
                   ..#...#...
                   #####...#.
                   #..#.#.#.#
                   ...#.#.#..
                   ##.#...##.
                   ..##.##.##
                   ###.##.#..

                   Tile 2473:
                   #....####.
                   #..#.##...
                   #.##..#...
                   ######.#.#
                   .#...#.#.#
                   .#########
                   .###.#..#.
                   ########.#
                   ##...##.#.
                   ..###.#.#.

                   Tile 2971:
                   ..#.#....#
                   #...###...
                   #.#.###...
                   ##.##..#..
                   .#####..##
                   .#..####.#
                   #..#.#..#.
                   ..####.###
                   ..#.#.###.
                   ...#.#.#.#

                   Tile 2729:
                   ...#.#.#.#
                   ####.#....
                   ..#.#.....
                   ....#..#.#
                   .##..##.#.
                   .#.####...
                   ####.#.#..
                   ##.####...
                   ##..#.##..
                   #.##...##.

                   Tile 3079:
                   #.#.#####.
                   .#..######
                   ..#.......
                   ######....
                   ####.#..#.
                   .#...#.##.
                   #.#####.##
                   ..#.###...
                   ..#.......
                   ..#.###...
                   """;

// Plan for algorithm
// Have tile object (2d array of bools(?)) - Done
// Create a list of tile objects from the input - Done
// Have an function that generates all 8 permutations of a tile - Done
// Have a function that can identify if 2 tiles match on a particular border (top, bottom, left or right) - Done
// Use a backtracking algorithm to try and brute force combinations of tiles together and see if the full picture can be assembled - Done

// Part 2
// Create an image class to hold the final image from the grid tiles
// Create a function that searches for monsters in the image and marks pixels that are part of the sea monster
// - The function should search all 8 permutations of the image, only 1 should contain sea monsters
// Count '#' pixels that are not part of any sea monsters

var tiles = ParseInput(input);
var imageWidth = (int)Math.Sqrt(tiles.Count);
var grid = new Tile?[imageWidth, imageWidth];

bool success = PlaceTiles(grid, tiles);

if (!success)
{
    Console.WriteLine("Failed to place all tiles");
    return;
}

var corners = new[]
    { grid[0, 0], grid[0, imageWidth - 1], grid[imageWidth - 1, 0], grid[imageWidth - 1, imageWidth - 1] };

var product = corners.Aggregate((long)1, (p, t) => p *= t.Id);

Console.WriteLine($"Product of corners: {product}");

var image = new Image(grid);

int roughSeas = image.SearchForSeaMonsters();

Console.WriteLine($"Rough sea count: {roughSeas}");


return;


static bool PlaceTiles(Tile?[,] grid, List<Tile> availableTiles)
{
    (int x, int y) = GetNextFreeSlot(grid);
    if ((x, y) == (-1, -1)) // No more slots to fill
        return true;
    
    var remainingTiles = new List<Tile>(availableTiles);
    foreach (var tile in availableTiles)
    {
        remainingTiles.Remove(tile);
        foreach (var tilePermutation in tile.GeneratePermutations())
        {
            bool fitsWithAbove = y switch
            {
                0 => true,
                _ => tilePermutation.MatchesWith(grid[x, y - 1]!, Border.Top)
            };

            bool fitsWithLeft = x switch
            {
                0 => true,
                _ => tilePermutation.MatchesWith(grid[x - 1, y]!, Border.Left)
            };

            if (!fitsWithAbove || !fitsWithLeft)
                continue;
            
            grid[x, y] = tilePermutation;
            bool success = PlaceTiles(grid, remainingTiles);
            if (success)
                return true;

            grid[x, y] = null;
        }
        remainingTiles.Add(tile);
    }

    return false;
}

static (int X, int Y) GetNextFreeSlot(Tile?[,] grid)
{
    for (int y = 0; y < grid.GetLength(1); y++)
    {
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            if (grid[x, y] is null)
                return (x, y);
        }
    }

    return (-1, -1);
}


static List<Tile> ParseInput(string input)
{
    var rawTiles = input.Split($"{Environment.NewLine}{Environment.NewLine}", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    var tiles = new List<Tile>();

    foreach (string rawTile in rawTiles)
    {
        var lines = rawTile.Split(Environment.NewLine);
        int id = int.Parse(lines[0].Split(' ')[1][..^1]);
        var tile = new Tile(id);
        for (int y = 1; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[1].Length; x++)
            {
                tile.Pixels[x, y - 1] = lines[y][x] switch
                {
                    '#' => true,
                    '.' => false,
                    _ => throw new InvalidOperationException($"Unrecognised character: {lines[y][x]}")
                };
            }
        }
        
        tiles.Add(tile);
    }

    return tiles;
}