namespace Day20;

public class Tile
{
    private const int Size = 10;
    
    public long Id { get; }
    public bool[,] Pixels { get; } = new bool[Size, Size];

    public Tile(long id)
    {
        Id = id;
    }

    public IEnumerable<Tile> GeneratePermutations()
    {
        foreach (var rotated in GenerateRotatedPermutations())
        {
            yield return rotated;
            yield return rotated.Flip();
        }
    }

    private IEnumerable<Tile> GenerateRotatedPermutations()
    {
        var rotated = this;
        yield return rotated;
        for (int i = 0; i < 3; i++)
        {
            rotated = rotated.Rotate();
            yield return rotated;
        }
    }

    private Tile Copy()
    {
        var copy = new Tile(Id);
        for (int y = 0; y < Size; y++)
        {
            for (int x = 0; x < Size; x++)
            {
                copy.Pixels[x, y] = Pixels[x, y];
            }
        }

        return copy;
    }

    private Tile Flip()
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

    private Tile Rotate()
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

    private bool[] GetBorder(Border border)
    {
        bool[] output = new bool[Size];
        for (int i = 0; i < Size; i++)
        {
            output[i] = border switch
            {
                Border.Top => Pixels[i, 0],
                Border.Bottom => Pixels[i, Size - 1],
                Border.Left => Pixels[0, i],
                Border.Right => Pixels[Size - 1, i],
                _ => throw new ArgumentOutOfRangeException(nameof(border))
            };
        }

        return output;
    }

    public bool MatchesWith(Tile other, Border border)
    {
        if (other is null)
            throw new ArgumentNullException(nameof(other));
        
        var thisBorderPixels = GetBorder(border);
        var otherBorderPixels = other.GetBorder(border.Opposite());

        for (int i = 0; i < Size; i++)
        {
            if (thisBorderPixels[i] != otherBorderPixels[i])
                return false;
        }

        return true;
    }
}

public enum Border
{
    Top,
    Bottom,
    Left,
    Right
}

public static class BorderExtensions
{
    public static Border Opposite(this Border border)
    {
        return border switch
        {
            Border.Top => Border.Bottom,
            Border.Bottom => Border.Top,
            Border.Left => Border.Right,
            Border.Right => Border.Left,
            _ => throw new ArgumentOutOfRangeException(nameof(border))
        };
    }
}