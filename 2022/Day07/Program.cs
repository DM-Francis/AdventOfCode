using Directory = Day07.Directory;
using File = Day07.File;

var input = System.IO.File.ReadAllLines("input.txt");
var root = BuildDirectoryStructure(input);

var allDirectorySizes = new Dictionary<Directory, int>();
RecursivelyGetSizeAndAddToListOfSizes(root, allDirectorySizes);

// Part 1
int sumForAllDirectoriesLessThan100000 = allDirectorySizes.Values.Where(x => x <= 100000).Sum();
Console.WriteLine($"Sum for all directories less than 100,000: {sumForAllDirectoriesLessThan100000}");

// Part 2
int totalUnusedSpace = 70000000 - allDirectorySizes[root];
int sizeThatNeedsToBeFreed = 30000000 - totalUnusedSpace;
int smallestDirectorySizeToDelete = allDirectorySizes.Values.Where(x => x >= sizeThatNeedsToBeFreed).Order().First();

Console.WriteLine($"Smallest directory size that can be deleted: {smallestDirectorySizeToDelete}");

static Directory BuildDirectoryStructure(string[] input)
{
    var root = new Directory { Name = "/", Parent = null };
    var current = root;
    foreach (string line in input[1..])
    {
        if (IsCdCommand(line))
        {
            current = ExecuteCdCommand(line, current);
        }
        else if (IsListCommand(line))
        {
        }
        else
        {
            AddObjectToCurrentDirectory(line, current);
        }
    }

    return root;
} 

static bool IsCdCommand(string line) => line.StartsWith("$ cd");
static bool IsListCommand(string line) => line == "$ ls";

static Directory ExecuteCdCommand(string line, Directory currentDirectory)
{
    var directoryName = line.Split(" ")[2];
    if (directoryName == "..")
        return currentDirectory?.Parent ?? throw new DirectoryNotFoundException();

    var newDir = currentDirectory.Children.OfType<Directory>().FirstOrDefault(c => c.Name == directoryName);
    if (newDir is not null)
        return newDir;
    
    newDir = new Directory() { Name = directoryName, Parent = currentDirectory };
    currentDirectory.Children.Add(newDir);

    return newDir;
}

static void AddObjectToCurrentDirectory(string line, Directory currentDirectory)
{
    var split = line.Split(" ");
    if (line.StartsWith("dir"))
    {
        string dirName = split[1];
        if (currentDirectory.Children.OfType<Directory>().Any(x => x.Name == dirName))
            return;
        var newDir = new Directory() { Name = dirName, Parent = currentDirectory };
        currentDirectory.Children.Add(newDir);
    }
    else
    {
        int size = int.Parse(split[0]);
        string fileName = split[1];
        if (currentDirectory.Children.OfType<File>().Any(x => x.Name == fileName))
            return;
        var newFile = new File() { Name = fileName, Size = size, Parent = currentDirectory };
        currentDirectory.Children.Add(newFile);
    }
}

static int GetSizeOfDirectory(Directory dir)
{
    return dir.Children.Sum(child => child switch
    {
        Directory childDir => GetSizeOfDirectory(childDir),
        File childFile => childFile.Size,
        _ => throw new InvalidOperationException("Unrecognised ITreeObject type")
    });
}

static void RecursivelyGetSizeAndAddToListOfSizes(Directory dir, IDictionary<Directory, int> sizes)
{
    int size = GetSizeOfDirectory(dir);
    sizes[dir] = size;
    foreach (var childDir in dir.Children.OfType<Directory>())
    {
        RecursivelyGetSizeAndAddToListOfSizes(childDir, sizes);
    }
}