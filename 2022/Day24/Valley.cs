namespace Day24;

public class Valley
{
    public int Width => Walls.GetLength(0);
    public int Height => Walls.GetLength(1);
    
    public bool[,] Walls { get; }
    public List<Direction>[,] Blizzards { get; }
    
    public Position Start { get; set; }
    public Position Finish { get; set; }
    public int Time { get; private set; }
    
    public Valley(int width, int height)
    {
        Walls = new bool[width, height];
        Blizzards = InitializeBlizzardArray(width, height);
    }

    private static List<Direction>[,] InitializeBlizzardArray(int width, int height)
    {
        var blizzards = new List<Direction>[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                blizzards[x, y] = new List<Direction>();
            }
        }

        return blizzards;
    }

    public Tile GetTileAt(Position p) => GetTileAt(p.X, p.Y);

    public Tile GetTileAt(int x, int y)
    {
        if (Walls[x, y])
            return Tile.Wall;
        if (Blizzards[x,y].Count > 0)
            return Tile.Blizzard;
        if (Start.X == x && Start.Y == y)
            return Tile.Start;
        if (Finish.X == x && Finish.Y == y)
            return Tile.Finish;
        return Tile.Empty;
    }

    public void Advance1TimeStep()
    {
        var blizzardsSnapshot = CloneBlizzardArray(Blizzards);
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (Walls[x,y])
                    continue;

                foreach (var blizzard in blizzardsSnapshot[x,y])
                {
                    AdvanceBlizzard(blizzard, new Position(x, y));
                }
            }
        }

        Time++;
    }

    private void AdvanceBlizzard(Direction direction, Position p)
    {
        Blizzards[p.X, p.Y].Remove(direction);
        
        var next = direction switch
        {
            Direction.Up => new Position(p.X, p.Y - 1),
            Direction.Down => new Position(p.X, p.Y + 1),
            Direction.Left => new Position(p.X - 1, p.Y),
            Direction.Right => new Position(p.X + 1, p.Y),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

        if (GetTileAt(next) == Tile.Wall)
        {
            next = direction switch
            {
                Direction.Up => new Position(next.X, next.Y + Height - 2),
                Direction.Down => new Position(next.X, next.Y - Height + 2),
                Direction.Left => new Position(next.X + Width - 2, next.Y),
                Direction.Right => new Position(next.X - Width + 2, next.Y),
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }
        
        Blizzards[next.X, next.Y].Add(direction);
    }
    
    private static List<Direction>[,] CloneBlizzardArray(List<Direction>[,] blizzards)
    {
        var clone = new List<Direction>[blizzards.GetLength(0), blizzards.GetLength(1)];
        for (int x = 0; x < blizzards.GetLength(0); x++)
        {
            for (int y = 0; y < blizzards.GetLength(1); y++)
            {
                clone[x, y] = blizzards[x, y].ToList();
            }
        }

        return clone;
    }
}

public readonly record struct Position(int X, int Y);

public enum Tile
{
    Empty,
    Wall,
    Blizzard,
    Start,
    Finish,
}

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}