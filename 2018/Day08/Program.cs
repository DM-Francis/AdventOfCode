var input = File.ReadAllLines("input.txt")[0];

var numbers = input.Split(' ').Select(int.Parse).ToList();
numbers.Reverse();
var data = new Stack<int>(numbers);

var root = ProcessNextNode(data);

int metadataSum = root.GetMetadataSum();
Console.WriteLine($"Sum of all metadata: {metadataSum}");

int rootValue = root.GetValue();
Console.WriteLine($"Value of root node: {rootValue}");

static Node ProcessNextNode(Stack<int> data)
{
    var node = new Node();
    int childCount = data.Pop();
    int metadataCount = data.Pop();
    
    for (int i = 0; i < childCount; i++)
    {
        var child = ProcessNextNode(data);
        child.Parent = node;
        node.ChildNodes.Add(child);
    }

    for (int i = 0; i < metadataCount; i++)
    {
        node.Metadata.Add(data.Pop());
    }

    return node;
}


public class Node
{
    public Node? Parent { get; set; }
    
    public List<Node> ChildNodes { get; } = new();
    public List<int> Metadata { get; } = new();

    public int GetMetadataSum()
    {
        int childSum = ChildNodes.Count > 0 ? ChildNodes.Sum(n => n.GetMetadataSum()) : 0;
        return childSum + Metadata.Sum();
    }

    public int GetValue()
    {
        if (ChildNodes.Count == 0)
            return Metadata.Sum();

        int valueSum = 0;
        foreach (int reference in Metadata)
        {
            if (reference <= ChildNodes.Count)
                valueSum += ChildNodes[reference - 1].GetValue();
        }

        return valueSum;
    }
}