namespace Day22;

public static class Extensions
{
    public static Facing RotateClockwise(this Facing facing)
    {
        return facing switch
        {
            Facing.Right => Facing.Down,
            Facing.Down => Facing.Left,
            Facing.Left => Facing.Up,
            Facing.Up => Facing.Right,
            _ => throw new ArgumentOutOfRangeException(nameof(facing))
        };
    }
    
    public static Facing RotateClockwise(this Facing facing, int times)
    {
        var result = facing;
        for (int i = 0; i < times; i++)
        {
            result = result.RotateClockwise();
        }

        return result;
    }
    
    public static Facing RotateCounterClockwise(this Facing facing)
    {
        return facing switch
        {
            Facing.Right => Facing.Up,
            Facing.Down => Facing.Right,
            Facing.Left => Facing.Down,
            Facing.Up => Facing.Left,
            _ => throw new ArgumentOutOfRangeException(nameof(facing))
        };
    }

    public static Facing Opposite(this Facing facing)
    {
        return facing switch
        {
            Facing.Right => Facing.Left,
            Facing.Left => Facing.Right,
            Facing.Up => Facing.Down,
            Facing.Down => Facing.Up,
            _ => throw new ArgumentOutOfRangeException(nameof(facing))
        };
    }
}