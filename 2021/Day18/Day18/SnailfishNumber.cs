namespace Day18;

public class SnailfishNumber
{
    public int GetMagnitude() => Root?.GetMagnitude() ?? throw new InvalidOperationException("Root cannot be null");
    
    public Pair? Root { get; init; }

    public void Reduce()
    {
        if (Root is null) throw new InvalidOperationException("Root cannot be null to reduce");

        var targets = GetNextReductionTargets();
        do
        {
            if (targets.ToExplode is not null)
            {
                targets.ToExplode.Explode();
                targets = GetNextReductionTargets();
                continue;
            }

            if (targets.ToSplit is not null)
            {
                targets.ToSplit.Split();
                targets = GetNextReductionTargets();
            }
        } while (targets.ToExplode is not null || targets.ToSplit is not null);
    }

    private (Pair? ToExplode, Number? ToSplit) GetNextReductionTargets()
    {
        var toExplode = Root?.GetAllSubPairs().FirstOrDefault(p => p.Depth >= 4);
        var toSplit = Root?.GetAllNumbers().FirstOrDefault(n => n.Value >= 10);
        return (toExplode, toSplit);
    }
    
    public static SnailfishNumber operator +(SnailfishNumber a, SnailfishNumber b)
    {
        var sum = new SnailfishNumber
        {
            Root = new Pair { Left = a.Root, Right = b.Root }
        };
        
        sum.Root.UpdateParentProperties();
        sum.Reduce();

        return sum;
    }

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

    public override string? ToString()
    {
        return Root?.ToString() ?? base.ToString();
    }
}

public class Pair : Node
{
    public override int GetMagnitude() => Left?.GetMagnitude() * 3 + Right?.GetMagnitude() * 2 ??
                                          throw new InvalidOperationException(
                                              "Left and Right can't be null to get magnitude");

    public Node? Left { get; set; }
    public Node? Right { get; set; }

    public Node? GetSide(Side side)
    {
        return side switch
        {
            Side.Left => Left,
            Side.Right => Right,
            _ => throw new ArgumentOutOfRangeException(nameof(side))
        };
    }

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

    public void UpdateParentProperties()
    {
        foreach (var child in new[] { Left, Right })
        {
            if (child is null)
                continue;
            
            child.Parent = this;
            if (child is Pair pair)
                pair.UpdateParentProperties();
        }
    }

    public IEnumerable<Pair> GetAllSubPairs()
    {
        foreach (var node in new[] { Left, Right })
        {
            if (node is not Pair pair)
                continue;
            yield return pair;
            foreach (var subPair in pair.GetAllSubPairs())
                yield return subPair;
        }
    }

    public IEnumerable<Number> GetAllNumbers()
    {
        foreach (var node in new[] { Left, Right })
        {
            switch (node)
            {
                case Number number:
                    yield return number;
                    break;
                case Pair pair:
                    foreach (var subNumber in pair.GetAllNumbers())
                        yield return subNumber;
                    break;
            }
        }
    }

    public override string ToString()
    {
        return $"[{Left},{Right}]";
    }

    public void Explode()
    {
        if (Left is not Number lNum || Right is not Number rNum)
            throw new InvalidOperationException("Can only explode pair with 2 numbers");
        if (Parent is null)
            throw new InvalidOperationException("Exploding pairs must have a parent");
        
        var left = GetNumberToSide(Side.Left);
        if (left is not null)
            left.Value += lNum.Value;

        var right = GetNumberToSide(Side.Right);
        if (right is not null)
            right.Value += rNum.Value;

        var zero = new Number { Value = 0, Parent = Parent };
        if (Parent.Left == this)
            Parent.Left = zero;
        if (Parent.Right == this)
            Parent.Right = zero;
    }

    private Number? GetNumberToSide(Side side)
    {
        if (Parent is null)
            return null;
        
        if (Parent.GetSide(side) == this)
        {
            return Parent.GetNumberToSide(side);
        }

        var current = Parent.GetSide(side);
        while (true)
        {
            switch (current)
            {
                case Number number:
                    return number;
                case Pair pair:
                    current = pair.GetSide(side.Opposite());
                    break;
                case null:
                    throw new InvalidOperationException("null node found in tree");
            }
        }
    }
}

public class Number : Node
{
    public override int GetMagnitude() => Value;

    public int Value { get; set; }

    public override string ToString()
    {
        return Value.ToString();
    }

    public void Split()
    {
        if (Parent is null) throw new InvalidOperationException("Number must have a parent to be split");
        
        var left = (int)Math.Floor((double)Value / 2);
        var right = (int)Math.Ceiling((double)Value / 2);
        var newPair = new Pair
        {
            Left = new Number { Value = left },
            Right = new Number { Value = right },
            Parent = Parent
        };

        newPair.Left.Parent = newPair;
        newPair.Right.Parent = newPair;

        if (Parent.Left == this)
            Parent.Left = newPair;
        if (Parent.Right == this)
            Parent.Right = newPair;
    }
}

public abstract class Node
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

    public abstract int GetMagnitude();
}

public enum Side
{
    Left,
    Right
}

public static class Extensions
{
    public static Side Opposite(this Side side)
    {
        return side switch
        {
            Side.Left => Side.Right,
            Side.Right => Side.Left,
            _ => throw new ArgumentOutOfRangeException(nameof(side))
        };
    }
}