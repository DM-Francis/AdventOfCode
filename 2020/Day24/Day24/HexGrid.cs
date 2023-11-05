namespace Day24;

public class HexGrid
{
    public Dictionary<TilePosition, Colour> Hexes { get; } = new();

    public int BlackTiles => Hexes.Values.Count(x => x == Colour.Black);

    public Colour this[TilePosition position]
    {
        get
        {
            if (Hexes.TryGetValue(position, out var colour))
                return colour;

            Hexes[position] = Colour.White;
            return Colour.White;
        }
        set => Hexes[position] = value;
    }

    public IEnumerable<TilePosition> AllTilePositions() => Hexes.Keys;
}

public record TilePosition(int A, int X, int Y)
{
    public TilePosition GetNeighbourInDirection(Direction direction)
    {
        return direction switch
        {
            Direction.E => new TilePosition(A, X + 1, Y),
            Direction.W => new TilePosition(A, X - 1, Y),
            Direction.NE => new TilePosition(1 - A, X + A, Y + 1 - A),
            Direction.NW => new TilePosition(1 - A, X - 1 + A, Y + 1 - A),
            Direction.SE => new TilePosition(1 - A, X + A, Y - A),
            Direction.SW => new TilePosition(1 - A, X - 1 + A, Y - A),
            _ => throw new ArgumentOutOfRangeException(nameof(direction))
        };
    }

    public IEnumerable<TilePosition> GetAllNeighbours()
    {
        yield return GetNeighbourInDirection(Direction.E);
        yield return GetNeighbourInDirection(Direction.W);
        yield return GetNeighbourInDirection(Direction.NE);
        yield return GetNeighbourInDirection(Direction.NW);
        yield return GetNeighbourInDirection(Direction.SE);
        yield return GetNeighbourInDirection(Direction.SW);
    }
}

public enum Direction
{
    E,
    W,
    NE,
    NW,
    SE,
    SW
}

public enum Colour { White, Black }

public static class Extensions
{
    public static Colour Flip(this Colour colour)
    {
        return colour switch
        {
            Colour.White => Colour.Black,
            Colour.Black => Colour.White,
            _ => throw new ArgumentOutOfRangeException(nameof(colour))
        };
    }
}