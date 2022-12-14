using System.Numerics;

namespace Day14;

public class Cave
{
    public int MaxX { get; }
    public int MaxY { get; }
    
    private readonly Square[,] _grid;

    public bool IsFull { get; set; } = false;

    public Square this[Vector2 position]
    {
        get => _grid[(int)position.X, (int)position.Y];
        set => _grid[(int)position.X, (int)position.Y] = value;
    }


    public Cave(string input, bool withFloor = false)
    {
        var allCoordStrings = input.Split(new [] {"\n", "->"}, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        var allCoords = allCoordStrings.Select(PositionFromString).ToList();

        MaxY = (int)allCoords.Max(c => c.Y);
        if (withFloor)
            MaxY += 2;
        
        MaxX = withFloor ? 500 + MaxY + 10 : (int)allCoords.Max(c => c.X);

        _grid = CreateGridFromInput(input, MaxX, MaxY);
        
        if (withFloor)
            AddFloorToGrid(_grid, MaxY);
    }

    public int GetSandCount()
    {
        int sandCount = 0;
        for (int y = 0; y < _grid.GetLength(1); y++)
        {
            for (int x = 0; x < _grid.GetLength(0); x++)
            {
                if (_grid[x, y] == Square.Sand)
                    sandCount++;
            }
        }

        return sandCount;
    }
    
    private static Square[,] CreateGridFromInput(string input, int maxX, int maxY)
    {
        var grid = new Square[maxX + 1, maxY + 1];

        var inputLines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        foreach (string line in inputLines)
        {
            var rockVertexStrings = line.Split("->", StringSplitOptions.TrimEntries);
            var vertices = rockVertexStrings.Select(PositionFromString).ToList();
            var current = vertices[0];
    
            foreach (var next in vertices.Skip(1))
            {
                var diff = Vector2.Normalize(next - current); // Will always be (1,0),(0,1),(-1,0) or (0,-1) due to the input
        
                while (current != next)
                {
                    grid[(int)current.X, (int)current.Y] = Square.Rock;
                    current += diff;
                }
            }

            grid[(int)current.X, (int)current.Y] = Square.Rock;
        }

        return grid;
    }

    private static void AddFloorToGrid(Square[,] grid, int floorY)
    {
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            grid[x, floorY] = Square.Rock;
        }
    }
    
    private static Vector2 PositionFromString(string value)
    {
        var split = value.Split(',');
        int x = int.Parse(split[0]);
        int y = int.Parse(split[1]);
        return new Vector2(x, y);
    }
    
    public void Render()
    {
        Console.Clear();
        int minX = GetMinXWithoutFloor();
        for (int y = 0; y < _grid.GetLength(1); y++)
        {
            for (int x = minX; x < _grid.GetLength(0); x++)
            {
                char output = _grid[x, y] switch
                {
                    Square.Empty => '.',
                    Square.Rock => '#',
                    Square.Sand => 'o',
                    _ => throw new InvalidOperationException("Unrecognised square enum value")
                };

                if (y == 0 && x == 500)
                    output = '+';
            
                Console.Write(output);
            }

            Console.WriteLine();
        }
    }

    private int GetMinXWithoutFloor()
    {
        int minX = MaxX;
        for (int x = 0; x < _grid.GetLength(0); x++)
        {
            for (int y = 0; y < _grid.GetLength(1) - 1; y++)
            {
                if (_grid[x, y] != Square.Empty && x < minX)
                    minX = x;
            }
        }

        return minX;
    }
}