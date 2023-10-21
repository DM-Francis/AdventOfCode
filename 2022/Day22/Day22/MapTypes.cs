namespace Day22;

public readonly record struct Position(int X, int Y)
{
    public Position MoveInDirection(Facing direction, int n = 1)
    {
        return direction switch
        {
            Facing.Right => this with { X = this.X + n },
            Facing.Down => this with { Y = this.Y + n },
            Facing.Left => this with { X = this.X - n },
            Facing.Up => this with { Y = this.Y - n },
            _ => throw new ArgumentOutOfRangeException(nameof(direction))
        };
    }
    
    public int ManhattanDistanceTo(Position other)
    {
        return Math.Abs(this.X - other.X) + Math.Abs(this.Y - other.Y);
    }
}

public readonly record struct Location(Position Position, Facing Facing)
{
    public Location TurnLeft()
    {
        var newFacing = Facing switch
        {
            Facing.Right => Facing.Up,
            Facing.Down => Facing.Right,
            Facing.Left => Facing.Down,
            Facing.Up => Facing.Left,
            _ => throw new ArgumentOutOfRangeException(nameof(Facing))
        };

        return this with { Facing = newFacing };
    }
    
    public Location TurnRight()
    {
        var newFacing = Facing switch
        {
            Facing.Right => Facing.Down,
            Facing.Down => Facing.Left,
            Facing.Left => Facing.Up,
            Facing.Up => Facing.Right,
            _ => throw new ArgumentOutOfRangeException(nameof(Facing))
        };

        return this with { Facing = newFacing };
    }
    
    public static Location Create(int x, int y, Facing facing)
    {
        return new Location(new Position(x, y), facing);
    }
}

public readonly record struct Edge(Position Start, Position End, Facing Normal)
{
    public int Length => Start.ManhattanDistanceTo(End);
    
    public IEnumerable<Position> GetPositions()
    {
        var current = Start;
        for (int i = 0; i <= Length; i++)
        {
            yield return current;
            current = current.MoveInDirection(Normal.RotateClockwise());
        }
    }
    
    public IEnumerable<Position> GetPositionsReversed()
    {
        var current = End;
        for (int i = 0; i <= Length; i++)
        {
            yield return current;
            current = current.MoveInDirection(Normal.RotateCounterClockwise());
        }
    }
}

public enum MapSquare
{
    Empty,
    Open,
    Wall
}

public enum Facing
{
    Right = 0,
    Down = 1,
    Left = 2,
    Up = 3
}