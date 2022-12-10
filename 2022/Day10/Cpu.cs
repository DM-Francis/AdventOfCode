namespace Day10;

public class Cpu
{
    private readonly List<int> _registerHistory = new() { 1 };
    private int X => _registerHistory[^1];

    public IReadOnlyList<int> RegisterHistory => _registerHistory.AsReadOnly();

    public void ApplyOperation(IOperation operation)
    {
        switch (operation)
        {
            case AddX add:
                SetRegisterForCycle(X);
                SetRegisterForCycle(X + add.Value);
                break;
            case Noop:
                SetRegisterForCycle(X);
                break;
        }
    }

    public int GetSignalStrengthDuringCycle(int cycleNumber) => _registerHistory[cycleNumber - 1] * cycleNumber;

    private void SetRegisterForCycle(int value)
    {
        _registerHistory.Add(value);
    }
}

public interface IOperation
{
}

public class AddX : IOperation
{
    public int Value { get; }

    public AddX(int value)
    {
        Value = value;
    }
}

public class Noop : IOperation
{
}