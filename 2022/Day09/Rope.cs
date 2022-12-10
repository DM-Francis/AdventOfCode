namespace Day09;

public class Rope
{
    private Position Head
    {
        get => _ropePositions[0];
        set => _ropePositions[0] = value;
    }
    private Position Tail => _ropePositions[_length - 1];

    private readonly int _length;
    private readonly Position[] _ropePositions;

    public HashSet<Position> TailPositionHistory { get; } = new();

    public Rope(int length = 2)
    {
        _length = length;
        _ropePositions = new Position[length];
    }

    public void MoveHead(Direction direction, int distance)
    {
        for (int i = 0; i < distance; i++)
        {
            MoveHeadOnce(direction);
        }
    }

    private void MoveHeadOnce(Direction direction)
    {
        var newHead = direction switch
        {
            Direction.Left => Head with { X = Head.X - 1 },
            Direction.Right => Head with { X = Head.X + 1 },
            Direction.Up => Head with { Y = Head.Y + 1 },
            Direction.Down => Head with { Y = Head.Y - 1 },
            _ => throw new ArgumentOutOfRangeException(nameof(direction))
        };

        Head = newHead;
        UpdateRestOfRope();
    }

    private void UpdateRestOfRope()
    {
        for (int i = 1; i < _length; i++)
        {
            UpdateRopeSegment(i);
        }

        TailPositionHistory.Add(Tail);
    }

    private void UpdateRopeSegment(int index)
    {
        var previous = _ropePositions[index - 1];
        var current = _ropePositions[index];
        
        int xDiff = previous.X - current.X;
        int yDiff = previous.Y - current.Y;

        if (Math.Abs(xDiff) <= 1 && Math.Abs(yDiff) <= 1) // Touching
            return;
        
        _ropePositions[index] = new Position(current.X + Math.Sign(xDiff), current.Y + Math.Sign(yDiff));
    }
}

public record struct Position(int X, int Y);

public enum Direction
{
    Left,
    Right,
    Up,
    Down
}