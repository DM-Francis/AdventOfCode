var inputNumbers = File.ReadLines("input.txt").Select(long.Parse).ToList();
var exampleNumbers = new List<long> { 1, 2, -3, 3, -2, 0, 4 };
var testNumbers = new List<long> { 4, -5, -1, 7, 4, 5 };
var numbers = inputNumbers;

Console.WriteLine($"Total count: {numbers.Count}.  Distinct count: {numbers.Distinct().Count()}");

var numberCircle = new LinkedList<long>(numbers);
var nodesToProcess = GetListOfNodes(numberCircle);

// Part 1
foreach(var node in nodesToProcess)
{
    MoveNumberThroughCircle(node, numberCircle);
}

var zero = numberCircle.Find(0)!;
var oneThousand = GetNodeNStepsForward(zero, numberCircle, 1000);
var twoThousand = GetNodeNStepsForward(zero, numberCircle, 2000);
var threeThousand = GetNodeNStepsForward(zero, numberCircle, 3000);

Console.WriteLine($"Sum of grove coordinates: {oneThousand.Value + twoThousand.Value + threeThousand.Value}");

// Part 2
var largeNumberCircle = new LinkedList<long>(numbers.Select(n => n * 811589153));
nodesToProcess = GetListOfNodes(largeNumberCircle);

for (int i = 0; i < 10; i++)
{
    foreach (var node in nodesToProcess)
    {
        MoveNumberThroughCircle(node, largeNumberCircle);
    }
}

zero = largeNumberCircle.Find(0)!;
oneThousand = GetNodeNStepsForward(zero, largeNumberCircle, 1000);
twoThousand = GetNodeNStepsForward(zero, largeNumberCircle, 2000);
threeThousand = GetNodeNStepsForward(zero, largeNumberCircle, 3000);

Console.WriteLine($"Sum of grove coordinates: {oneThousand.Value + twoThousand.Value + threeThousand.Value}");


static void MoveNumberThroughCircle(LinkedListNode<long> number, LinkedList<long> circle)
{
    if (number.Value >= 0)
        MoveNumberForwards(number, circle, number.Value % (circle.Count - 1));
    else
        MoveNumberBackwards(number, circle, -number.Value % (circle.Count - 1));
}

static void MoveNumberForwards(LinkedListNode<long> number, LinkedList<long> circle, long amount)
{
    if (amount == 0)
        return;

    var next = number.Next ?? circle.First;
    circle.Remove(number);
    for (int i = 1; i < amount; i++)
    {
        next = next?.Next ?? circle.First;
    }

    if (next is null)
        throw new InvalidOperationException("Somehow failed to navigate through the linked list");
    
    circle.AddAfter(next, number);
}

static void MoveNumberBackwards(LinkedListNode<long> number, LinkedList<long> circle, long amount)
{
    if (amount == 0)
        return;

    var previous = number.Previous ?? circle.Last;
    circle.Remove(number);
    for (int i = 1; i < amount; i++)
    {
        previous = previous?.Previous ?? circle.Last;
    }

    if (previous is null)
        throw new InvalidOperationException("Somehow failed to navigate through the linked list");
    
    circle.AddBefore(previous, number);
}

static LinkedListNode<long> GetNodeNStepsForward(LinkedListNode<long> current, LinkedList<long> circle, int n)
{
    for (int i = 0; i < n; i++)
    {
        current = current?.Next ?? circle.First!;
    }

    return current;
}

static List<LinkedListNode<long>> GetListOfNodes(LinkedList<long> circle)
{
    var output = new List<LinkedListNode<long>>();
    var current = circle.First;
    while (current is not null)
    {
        output.Add(current);
        current = current.Next;
    }

    return output;
}