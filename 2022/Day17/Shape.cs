using System.Text;

namespace Day17;

public class Shape
{
    private readonly bool[,] _shape;  // x and y coordinates are measured from the bottom left of the shape, with x going to the left, and y going up

    public int Height => _shape.GetLength(1);
    public int Width => _shape.GetLength(0);

    public bool this[int x, int y] => _shape[x, y];
    
    private Shape(bool[,] shape)
    {
        _shape = new bool[shape.GetLength(0), shape.GetLength(1)];
        for (int x = 0; x < shape.GetLength(0); x++)
        {
            for (int y = 0; y < shape.GetLength(1); y++)
            {
                _shape[x, y] = shape[x, y];
            }
        }
    }

    public static Shape FromString(string rawShape)
    {
        var lines = rawShape.Split(Environment.NewLine);
        var shape = new bool[lines[0].Length, lines.Length];
        for (int i = 0; i < lines.Length; i++)
        {
            int y = lines.Length - i - 1;
            var line = lines[i];
            for (int x = 0; x < line.Length; x++)
            {
                shape[x, y] = line[x] == '#';
            }
        }

        return new Shape(shape);
    }

    public override string ToString()
    {
        var output = new StringBuilder();
        for (int y = _shape.GetUpperBound(1); y >= 0; y--)
        {
            for (int x = 0; x < _shape.GetLength(0); x++)
            {
                output.Append(_shape[x,y] ? '#' : '.');
            }

            if (y > 0)
                output.AppendLine();
        }

        return output.ToString();
    }
}