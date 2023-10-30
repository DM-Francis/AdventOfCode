namespace Day20;

public class Image
{
    private const string RawSeaMonster = """
                                                           # 
                                         #    ##    ##    ###
                                          #  #  #  #  #  #   
                                         """;

    private static readonly bool[,] SeaMonster = ParseSeaMonster();

    private static bool[,] ParseSeaMonster()
    {
        var monsterSplit = RawSeaMonster.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        var output = new bool[monsterSplit[0].Length, monsterSplit.Length];
        for (int x = 0; x < output.GetLength(0); x++)
        {
            for (int y = 0; y < output.GetLength(1); y++)
            {
                output[x, y] = monsterSplit[y][x] == '#';
            }
        }

        return output;
    }
    
    public int Size { get; }
    public bool[,] Pixels { get; }
    
    public HashSet<(int X, int Y)> SeaMonsterPixels { get; private set; }

    private Image(int size)
    {
        Size = size;
        Pixels = new bool[Size, Size];
    }
    
    public Image(Tile[,] grid)
    {
        var tileSize = grid[0, 0].Pixels.GetLength(0);
        var trimmedTileSize = tileSize - 2;
        var gridSize = grid.GetLength(0);

        Size = trimmedTileSize * gridSize;
        Pixels = new bool[Size, Size];
        
        for (int gridX = 0; gridX < gridSize; gridX++)
        {
            for (int gridY = 0; gridY < gridSize; gridY++)
            {
                for (int tileX = 1; tileX < tileSize - 1; tileX++)
                {
                    for (int tileY = 1; tileY < tileSize - 1; tileY++)
                    {
                        Pixels[gridX * trimmedTileSize + tileX - 1, gridY * trimmedTileSize + tileY - 1] =
                            grid[gridX, gridY].Pixels[tileX, tileY];
                    }
                }
            }
        }
    }

    public int SearchForSeaMonsters()
    {
        int monsterLength = SeaMonster.GetLength(0);
        int monsterHeight = SeaMonster.GetLength(1);

        foreach (var permutation in GeneratePermutations())
        {
            permutation.SeaMonsterPixels = new HashSet<(int X, int Y)>();
            for (int x = 0; x < Size - monsterLength + 1; x++)
            {
                for (int y = 0; y < Size - monsterHeight + 1; y++)
                {
                    permutation.CheckForSeaMonsterStartingAt(x, y);
                }
            }

            if (permutation.SeaMonsterPixels.Count > 0)
            {
                int roughSeaCount = 0;
                for (int x = 0; x < Size; x++)
                {
                    for (int y = 0; y < Size; y++)
                    {
                        if (permutation.Pixels[x, y] && !permutation.SeaMonsterPixels.Contains((x, y)))
                            roughSeaCount++;
                    }
                }

                return roughSeaCount;
            }
        }

        throw new InvalidOperationException("No sea monsters found");
    }

    private bool CheckForSeaMonsterStartingAt(int x, int y)
    {
        var smPixels = new List<(int X, int Y)>();
        for (int smX = 0; smX < SeaMonster.GetLength(0); smX++)
        {
            for (int smY = 0; smY < SeaMonster.GetLength(1); smY++)
            {
                if (!SeaMonster[smX, smY])
                    continue;
                
                if (!Pixels[x + smX, y + smY])
                    return false;

                smPixels.Add((x + smX, y + smY));
            }
        }

        foreach(var pixel in smPixels)
            SeaMonsterPixels.Add(pixel);
        return true;
    }
    
    public IEnumerable<Image> GeneratePermutations()
    {
        foreach (var rotated in GenerateRotatedPermutations())
        {
            yield return rotated;
            yield return rotated.Flip();
        }
    }

    private IEnumerable<Image> GenerateRotatedPermutations()
    {
        var rotated = this;
        yield return rotated;
        for (int i = 0; i < 3; i++)
        {
            rotated = rotated.Rotate();
            yield return rotated;
        }
    }

    private Image Copy()
    {
        var copy = new Image(Size);
        for (int y = 0; y < Size; y++)
        {
            for (int x = 0; x < Size; x++)
            {
                copy.Pixels[x, y] = Pixels[x, y];
            }
        }

        return copy;
    }

    private Image Flip()
    {
        var flipped = Copy();
        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
            {
                (int newX, int newY) = (Size - x - 1, y);
                flipped.Pixels[newX, newY] = Pixels[x, y];
            }
        }

        return flipped;
    }

    private Image Rotate()
    {
        var rotated = Copy();
        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
            {
                (int newX, int newY) = (Size - y - 1, x);
                rotated.Pixels[newX, newY] = Pixels[x, y];
            }
        }

        return rotated;
    }
}