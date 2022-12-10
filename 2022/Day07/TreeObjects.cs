namespace Day07;

public interface ITreeObject
{
    string Name { get; }
    Directory? Parent { get; }
}

public class Directory : ITreeObject
{
    public required string Name { get; init; }
    public required Directory? Parent { get; init; }
    public ICollection<ITreeObject> Children { get; } = new List<ITreeObject>();
}

public class File : ITreeObject
{
    public required string Name { get; init; }
    public required int Size { get; init; }
    public required Directory Parent { get; init; }
}