using System.Diagnostics;

var input = File.ReadAllLines("input.txt");
var testInput = new string[]
{
    "start-A",
    "start-b",
    "A-c",
    "A-b",
    "b-d",
    "A-end",
    "b-end",
};


var allCaveLinks = GetCaveLinksFromInput(input);
var visitedCaves = new HashSet<string>();

int paths = CountAllPathsToEnd("start", allCaveLinks, visitedCaves, false);

Console.WriteLine($"Path count: {paths}");

static Dictionary<string, List<string>> GetCaveLinksFromInput(string[] input)
{
    var allCaveLinks = new Dictionary<string, List<string>>();

    foreach (string line in input)
    {
        string[] caves = line.Split('-');

        if (!allCaveLinks.ContainsKey(caves[0]))
            allCaveLinks[caves[0]] = new List<string>();

        if (!allCaveLinks.ContainsKey(caves[1]))
            allCaveLinks[caves[1]] = new List<string>();

        allCaveLinks[caves[0]].Add(caves[1]);
        allCaveLinks[caves[1]].Add(caves[0]);
    }

    return allCaveLinks;
}

static int CountAllPathsToEnd(string currentCave, Dictionary<string, List<string>> allCaveLinks, HashSet<string> visitedCaves, bool usedExtraVisit)
{
    int pathCount = 0;
    bool hasVisitedBefore = !visitedCaves.Add(currentCave);

    if (char.IsLower(currentCave[0]) && hasVisitedBefore)
        usedExtraVisit = true;

    foreach (string cave in allCaveLinks[currentCave])
    {
        if (cave == "end")
        {
            pathCount++;
            continue;
        }

        if (CanVisitCave(cave, visitedCaves, usedExtraVisit))
        {
            pathCount += CountAllPathsToEnd(cave, allCaveLinks, visitedCaves, usedExtraVisit);
        }
    }

    if (!hasVisitedBefore)
        visitedCaves.Remove(currentCave);

    return pathCount;
}

static bool CanVisitCave(string cave, HashSet<string> visitedCaves, bool usedExtraVisit)
{
    if (cave == "start")
        return false;
    else if (char.IsUpper(cave[0]))
        return true;
    else if (!visitedCaves.Contains(cave))
        return true;
    else if (!usedExtraVisit)
        return true;
    else
        return false;
}