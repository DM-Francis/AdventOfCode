namespace Day18;

public class SnailfishNumber
{
    public Pair? Root { get; init; }

    public static SnailfishNumber Parse(string input)
    {
        var root = new Pair();
        var current = root;
        var side = Side.Left;
        foreach (char c in input[1..^1])
        {
            switch (c)
            {
                case '[':
                    var parent = current;
                    current = new Pair { Parent = parent };
                    parent?.SetSide(side, current);
                    side = Side.Left;
                    break;
                case ',':
                    side = Side.Right;
                    break;
                case ']':
                    if (current is null) throw new InvalidOperationException("current should not be null here");
                    current = current.Parent;
                    break;
                default:
                    int value = int.Parse(c.ToString());
                    var child = new Number { Value = value, Parent = current };
                    if (current is null) throw new InvalidOperationException("current should not be null here");
                    current.SetSide(side, child);
                    break;
            }
        }

        return new SnailfishNumber { Root = root };
    }
}

public class Pair : Node
{
    public Node? Left { get; set; }
    public Node? Right { get; set; }

    public void SetSide(Side side, Node node)
    {
        switch (side)
        {
            case Side.Left:
                Left = node;
                break;
            case Side.Right:
                Right = node;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(side));
        }
    }

    public void SetParentProperties()
    {
        foreach (var child in new[] { Left, Right })
        {
            if (child is null)
                continue;
            
            child.Parent = this;
            if (child is Pair pair)
                pair.SetParentProperties();
        }
    }

    public override string ToString()
    {
        return $"[{Left},{Right}]";
    }
}

public class Number : Node
{
    public int Value { get; set; }

    public override string ToString()
    {
        return Value.ToString();
    }
}

public class Node
{
    public Pair? Parent { get; set; }

    public int Depth
    {
        get
        {
            int depth = 0;
            var parent = Parent;
            while (parent is not null)
            {
                depth++;
                parent = parent.Parent;
            }

            return depth;
        }
    }
}

public enum Side
{
    Left,
    Right
}