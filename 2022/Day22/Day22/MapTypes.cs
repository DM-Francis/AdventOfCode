namespace Day22;

public readonly record struct Position(int X, int Y)
{
    public Position MoveInDirection(Facing direction)
    {
        return direction switch
        {
            Facing.Right => this with { X = this.X + 1 },
            Facing.Down => this with { Y = this.Y + 1 },
            Facing.Left => this with { X = this.X - 1 },
            Facing.Up => this with { Y = this.Y - 1 },
            _ => throw new ArgumentOutOfRangeException(nameof(direction))
        };
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