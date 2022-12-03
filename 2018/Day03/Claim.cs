namespace Day03;

public record Claim(
    int Id,
    int FromLeft,
    int FromTop,
    int Width,
    int Height)
{
    public static Claim FromString(string claimString)
    {
        string[] claimSplit = claimString.Split(" ");
        string idString = claimSplit[0];
        string positionString = claimSplit[2];
        string sizeString = claimSplit[3];

        int id = int.Parse(idString[1..]);

        var positionInts = positionString[..^1].Split(',').Select(int.Parse).ToList();
        int fromLeft = positionInts[0];
        int fromRight = positionInts[1];

        var sizeInts = sizeString.Split('x').Select(int.Parse).ToList();
        int width = sizeInts[0];
        int height = sizeInts[1];

        return new Claim(id, fromLeft, fromRight, width, height);
    }
}