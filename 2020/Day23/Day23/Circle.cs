namespace Day23;

public class Circle
{
    private readonly int _maxValue;
    private readonly LinkedList<int> _list = new();
    private readonly LinkedListNode<int>[] _jumpList;
    
    public LinkedListNode<int> Current { get; private set; }
    
    public Circle(string raw)
    {
        _jumpList = new LinkedListNode<int>[raw.Length + 1];
        foreach (char c in raw)
        {
            int value = c - '0';
            var node = _list.AddLast(value);
            _jumpList[value] = node;
            if (value > _maxValue)
                _maxValue = value;
        }

        Current = _list.First ?? throw new InvalidOperationException();
    }

    public Circle(string raw, int maxValue)
    {
        _jumpList = new LinkedListNode<int>[maxValue + 1];
        foreach (char c in raw)
        {
            int value = c - '0';
            var node = _list.AddLast(value);
            _jumpList[value] = node;
            if (value > _maxValue)
                _maxValue = value;
        }

        for (int i = _maxValue + 1; i <= maxValue; i++)
        {
            var node = _list.AddLast(i);
            _jumpList[i] = node;
        }
        
        _maxValue = maxValue;
        Current = _list.First ?? throw new InvalidOperationException();
    }

    private readonly LinkedListNode<int>[] _pickedUp = new LinkedListNode<int>[3];
    public void Move3AtCurrent()
    {
        var next1 = GetNext(Current);
        var next2 = GetNext(next1);
        var next3 = GetNext(next2);

        if (next1 is null || next2 is null || next3 is null) throw new InvalidOperationException();

        _pickedUp[2] = next1;
        _pickedUp[1] = next2;
        _pickedUp[0] = next3;

        int destination = Current.Value;
        do
        {
            destination--;
            if (destination < 1)
                destination = _maxValue;
        } while (_pickedUp[0].Value == destination || _pickedUp[1].Value == destination ||
                 _pickedUp[2].Value == destination);

        var destinationNode = _jumpList[destination];
        foreach (var node in _pickedUp)
        {
            _list.Remove(node);
            _list.AddAfter(destinationNode, node);
        }

        Current = GetNext(Current);
    }

    public IEnumerable<int> GetLabelsAfter1()
    {
        var oneNode = _list.Find(1) ?? throw new InvalidOperationException("Node '1' not found");
        var current = GetNext(oneNode);
        while (current.Value != 1)
        {
            yield return current.Value;
            current = GetNext(current);
        }
    }

    public (int a, int b) Get2LabelsClockwiseOf1()
    {
        var oneNode = _jumpList[1];
        var next1 = GetNext(oneNode);
        var next2 = GetNext(next1);
        return (next1.Value, next2.Value);
    }

    private LinkedListNode<int> GetNext(LinkedListNode<int> node)
    {
        return (node.Next ?? _list.First) ?? throw new InvalidOperationException();
    }
}