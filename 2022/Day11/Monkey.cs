namespace Day11;

public class Monkey
{
    public Queue<Worry> Items { get; }
    public Func<Worry, Worry> Operation { get; }
    public Func<Worry, int> Test { get; }
    
    public long InspectionCount { get; set; }

    public Monkey(IEnumerable<Worry> startingItems, Func<Worry, Worry> operation, Func<Worry, int> test)
    {
        Items = new Queue<Worry>(startingItems);
        Operation = operation;
        Test = test;
    }
}