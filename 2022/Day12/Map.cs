namespace Day12;

public class Map
{
    public int MaxX => Heights.GetUpperBound(0);
    public int MaxY => Heights.GetUpperBound(1);
    
    public int[,] Heights { get; }
    public Position StartingPosition { get; }
    public Position BestSignalPosition { get; }

    public Map(int[,] height, Position startingPosition, Position bestSignalPosition)
    {
        Heights = height;
        StartingPosition = startingPosition;
        BestSignalPosition = bestSignalPosition;
    }

    public IEnumerable<Position> GetReverseAccessiblePositionsFrom(Position p)
    {
        int currentHeight = Heights[p.X, p.Y];

        if (p.X > 0 && Heights[p.X - 1, p.Y] >= currentHeight - 1)
            yield return new Position(p.X - 1, p.Y);
        if (p.Y > 0 && Heights[p.X, p.Y - 1] >= currentHeight - 1)
            yield return new Position(p.X, p.Y - 1);
        if (p.X < MaxX && Heights[p.X + 1, p.Y] >= currentHeight - 1)
            yield return new Position(p.X + 1, p.Y);
        if (p.Y < MaxY && Heights[p.X, p.Y + 1] >= currentHeight - 1)
            yield return new Position(p.X, p.Y + 1);
    }
}

public record struct Position(int X, int Y);