var input = File.ReadAllLines("input.txt");

long sumOfExpressions = 0;
foreach (var line in input)
{
    var tokens = line.Replace(" ", "");
    string rpn = ConvertToReversePolishNotation(tokens, Precedence.AdditionOverMultiplication);
    sumOfExpressions += EvaluateReversePolishExpression(rpn);
}

Console.WriteLine($"Sum of evaluated expressions: {sumOfExpressions}");


static string ConvertToReversePolishNotation(string tokens, Precedence precedence)
{
    // Shunting yard algorithm
    var output = new Queue<char>();
    var operatorStack = new Stack<char>();
    
    foreach (char token in tokens)
    {
        switch (token)
        {
            case >= '0' and <= '9':
                output.Enqueue(token);
                break;
            case '+':
            case '*':
                while (operatorStack.TryPeek(out char previousOp) && previousOp != '(' && HasEqualOrGreaterPrecedenceThan(previousOp, token, precedence))
                    output.Enqueue(operatorStack.Pop());
                operatorStack.Push(token);
                break;
            case '(':
                operatorStack.Push(token);
                break;
            case ')':
                while (operatorStack.Peek() != '(')
                    output.Enqueue(operatorStack.Pop());
                operatorStack.Pop(); // Discard left parentheses
                break;
            default:
                throw new InvalidOperationException("Unrecognised token");     
        }
    }
    
    while (operatorStack.Count > 0)
        output.Enqueue(operatorStack.Pop());

    return new string(output.ToArray());
}

static long EvaluateReversePolishExpression(string rpnInput)
{
    var stack = new Stack<long>();

    foreach (char token in rpnInput)
    {
        long a;
        long b;
        switch (token)
        {
            case >= '0' and <= '9':
                int value = int.Parse(token.ToString());
                stack.Push(value);
                break;
            case '*':
                a = stack.Pop();
                b = stack.Pop();
                stack.Push(a * b);
                break;
            case '+':
                a = stack.Pop();
                b = stack.Pop();
                stack.Push(a + b);
                break;
        }
    }

    if (stack.Count != 1)
        throw new ArgumentException("Invalid Reverse Polish notation string", nameof(rpnInput));

    return stack.Pop();
}

static bool HasEqualOrGreaterPrecedenceThan(char value, char other, Precedence precedence)
{
    if (precedence == Precedence.Equal)
        return true;

    if (value == '*' && other == '+')
        return false;

    return true;
}

public enum Precedence { Equal, AdditionOverMultiplication }