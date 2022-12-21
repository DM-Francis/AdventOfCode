var input = File.ReadAllLines("input.txt");

var monkeyInputs = new Dictionary<string, string>();
foreach (var line in input)
{
    var split = line.Split(':', StringSplitOptions.TrimEntries);
    monkeyInputs[split[0]] = split[1];
}

var allMonkeys = new Dictionary<string, IMonkey>();
var root = GenerateMonkeyObject("root", monkeyInputs, false, allMonkeys);

long rootNumberYelled = root.YellNumber();

Console.WriteLine($"Number yelled by root monkey: {rootNumberYelled}");

// Part 2
var rootExpression = monkeyInputs["root"];
var rootSplit = rootExpression.Split(' ');
var leftName = rootSplit[0];
var rightName = rootSplit[2];

var allMonkeys2 = new Dictionary<string, IMonkey>();
var leftMonkey = GenerateMonkeyObject(leftName, monkeyInputs, true, allMonkeys2);
var rightMonkey = GenerateMonkeyObject(rightName, monkeyInputs, true, allMonkeys2);

if (leftMonkey.IsUnknown)
    leftMonkey.SetValue(rightMonkey.YellNumber());
else if (rightMonkey.IsUnknown)
    rightMonkey.SetValue(leftMonkey.YellNumber());

long humanNumber = ((Human)allMonkeys2["humn"]).Value.Value;

Console.WriteLine($"Human number to yell: {humanNumber}");


static IMonkey GenerateMonkeyObject(string name,
    IReadOnlyDictionary<string, string> monkeyInputs,
    bool partTwo,
    IDictionary<string, IMonkey> allMonkeys)
{
    if (partTwo && name == "humn")
    {
        var human = new Human();
        allMonkeys[human.Name] = human;
        return human;
    }

    var expression = monkeyInputs[name];
    if (long.TryParse(expression, out long value))
    {
        var numberMonkey = new NumberMonkey(name, value);
        allMonkeys[numberMonkey.Name] = numberMonkey;
        return numberMonkey;
    }

    var expressionSplit = expression.Split(' ');
    var operation = expressionSplit[1][0];
    var leftName = expressionSplit[0];
    var rightName = expressionSplit[2];

    var monkey = new OperationMonkey(name, operation)
    {
        Left = GenerateMonkeyObject(leftName, monkeyInputs, partTwo, allMonkeys),
        Right = GenerateMonkeyObject(rightName, monkeyInputs, partTwo, allMonkeys)
    };

    allMonkeys[monkey.Name] = monkey;

    return monkey;
}


public interface IMonkey
{
    string Name { get; }
    long YellNumber();
    bool IsUnknown { get; }
    void SetValue(long value);
}

public class Human : IMonkey
{
    public string Name => "humn";
    public long YellNumber() => throw new InvalidOperationException("Unknown number");
    public bool IsUnknown => Value is null;
    public long? Value { get; private set; }
    public void SetValue(long value) => Value = value;
}

public class NumberMonkey : IMonkey
{
    private readonly long _value;

    public string Name { get; }
    public long YellNumber() => _value;
    public bool IsUnknown => false;

    public void SetValue(long value) => throw new InvalidOperationException("Cannot set value on NumberMonkey");
    
    public NumberMonkey(string name, long value)
    {
        Name = name;
        _value = value;
    }
}

public class OperationMonkey : IMonkey
{
    private readonly Func<long, long, long> _operation;
    private readonly char _operationChar;
    
    public string Name { get; }
    public required IMonkey Left { get; init; }
    public required IMonkey Right { get; init; }

    public bool IsUnknown => Left.IsUnknown || Right.IsUnknown;

    public long YellNumber() => _operation(Left.YellNumber(), Right.YellNumber());

    public void SetValue(long value)
    {
        if (Left.IsUnknown && Right.IsUnknown)
            throw new InvalidOperationException("Both sides unknown");
        
        if (Right.IsUnknown) // L o X = value
        {
            long left = Left.YellNumber();
            long desiredRight = _operationChar switch
            {
                '+' => value - left, // L + x = value; x = value - L
                '-' => left - value, // L - x = value; x = L - value
                '*' => value / left, // L * x = value;  x = value / L
                '/' => left / value, // L / x = value; x = L / value
                _ => throw new ArgumentOutOfRangeException(nameof(_operationChar))
            };
            
            Right.SetValue(desiredRight);
        }
        else if (Left.IsUnknown) // X o R = value
        {
            long right = Right.YellNumber();
            long desiredLeft = _operationChar switch
            {
                '+' => value - right, // x + R = value; x = value - R
                '-' => value + right, // x - R = value; x = value + R
                '*' => value / right, // x * R = value;  x = value / R
                '/' => value * right, // x / R = value; x = value * R
                _ => throw new ArgumentOutOfRangeException(nameof(_operationChar))
            };
            
            Left.SetValue(desiredLeft);
        }
    }

    public OperationMonkey(string name, char operation)
    {
        Name = name;
        _operationChar = operation;
        _operation = operation switch
        {
            '+' => (l, r) => l + r,
            '-' => (l, r) => l - r,
            '*' => (l, r) => l * r,
            '/' => (l, r) => l / r,
            _ => throw new ArgumentOutOfRangeException(nameof(operation))
        };
    }
}