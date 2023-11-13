namespace Day20;

public class Image
{
    public int MinX { get; private set; }
    public int MaxX { get; private set; }
    public int MinY { get; private set; }
    public int MaxY { get; private set; }
    public Dictionary<Point, bool> Pixels { get; set; } = new();
    public bool OuterValue { get; private set; }

    public Image(IReadOnlyList<string> rawImage)
    {
        MinX = 0;
        MinY = 0;
        MaxX = rawImage[0].Length - 1;
        MaxY = rawImage.Count - 1;
        OuterValue = false;
        for (int x = 0; x <= MaxX; x++)
        {
            for (int y = 0; y <= MaxY; y++)
            {
                Pixels[new Point(x,y)] = rawImage[y][x] == '#';
            }
        }
    }

    public bool GetValue(int x, int y)
    {
        return Pixels.TryGetValue(new Point(x, y), out bool value) ? value : OuterValue;
    }

    public void ApplyEnhancement(IReadOnlyDictionary<int, bool> enhancementMap)
    {
        var newPixels = new Dictionary<Point, bool>();
        MinX--;
        MinY--;
        MaxX++;
        MaxY++;
        
        for (int x = MinX; x <= MaxX; x++)
        {
            for (int y = MinY; y <= MaxY; y++)
            {
                var p = new Point(x, y);
                int binaryValue = GetBinaryFromNeighbours(p);
                newPixels[p] = enhancementMap[binaryValue];
            }
        }
        
        var newOuter = OuterValue ? enhancementMap[511] : enhancementMap[0];
        Pixels = newPixels;
        OuterValue = newOuter;
    }

    private int GetBinaryFromNeighbours(Point p)
    {
        var values = new List<bool>(9);
        for (int y = p.Y - 1; y <= p.Y + 1; y++)
        {
            for (int x = p.X - 1; x <= p.X + 1; x++)
            {
                bool value = GetValue(x, y);
                values.Add(value);
            }
        }

        int binaryValue = 0;
        for (int i = 0; i < values.Count; i++)
        {
            binaryValue += values[i] ? 1 << (8 - i) : 0;
        }
        
        return binaryValue;
    }
}


public record struct Point(int X, int Y);