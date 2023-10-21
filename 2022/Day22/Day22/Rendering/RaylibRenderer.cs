using Raylib_cs;

namespace Day22.Rendering;

public class RaylibRenderer : IRenderer
{
    public void Draw(MapSquare[,] map, IReadOnlyList<Location> path)
    {
        try
        {
            const int origRow = 0;
            const int origCol = 0;
        
            DrawMap(map, origRow, origCol);
            DrawPath(path, origRow, origCol);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    
    private static void DrawMap(MapSquare[,] map, int origRow, int origCol)
    {
        for (int y = 0; y < map.GetLength(1); y++)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                var output = map[x, y] switch
                {
                    MapSquare.Empty => ' ',
                    MapSquare.Open => '.',
                    MapSquare.Wall => '#',
                    _ => throw new IndexOutOfRangeException()
                };

                RenderAt(output, x, y, Color.WHITE, origRow, origCol);
            }
        }
    }

    private static void DrawPath(IReadOnlyList<Location> path, int origRow, int origCol)
    {
        if (path.Count == 0)
            return;

        var toRender = path.TakeLast(500).ToList();
        foreach (var location in toRender)
        {
            var output = location.Facing switch
            {
                Facing.Right => '>',
                Facing.Down => 'v',
                Facing.Left => '<',
                Facing.Up => '^',
                _ => throw new IndexOutOfRangeException()
            };

            RenderAt(output, location.Position.X, location.Position.Y, Color.GREEN, origRow, origCol);
        }
        
        var last = path[^1];
        RenderAt('X', last.Position.X, last.Position.Y, Color.RED, origRow, origCol);
    }

    private static void RenderAt(char c, int x, int y, Color color, int origRow, int origCol)
    {
        int fontSize = 3;
        Raylib.DrawText(c.ToString(), origCol + x * fontSize, origRow + y * fontSize, fontSize, color);
    }
}