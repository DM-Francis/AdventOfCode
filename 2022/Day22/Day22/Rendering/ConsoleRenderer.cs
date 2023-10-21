namespace Day22.Rendering;

public class ConsoleRenderer : IRenderer
{
    public void Draw(MapSquare[,] map, IReadOnlyList<Location> path)
    {
        Console.Clear();
        int origRow = Console.CursorTop;
        int origCol = Console.CursorLeft;
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

                RenderAt(output, x, y, ConsoleColor.White, origRow, origCol);
            }
        }

        var previous = path[0];
        foreach (var location in path)
        {
            int diff = Math.Abs(location.Position.X - previous.Position.X) +
                       Math.Abs(location.Position.Y - previous.Position.Y);

            if (diff % 50 != 49 && diff > 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write('X');
                break;
            }

            previous = location;

            var output = location.Facing switch
            {
                Facing.Right => '>',
                Facing.Down => 'v',
                Facing.Left => '<',
                Facing.Up => '^',
                _ => throw new IndexOutOfRangeException()
            };
            
            RenderAt(output, location.Position.X, location.Position.Y, ConsoleColor.Green, origRow, origCol);
            // Thread.Sleep(10);
        }
        
        Console.SetCursorPosition(0, origRow + map.GetLength(1));
    }

    private static void RenderAt(char c, int x, int y, ConsoleColor color, int origRow, int origCol)
    {
        Console.SetCursorPosition(origCol + x, origRow + y);
        Console.ResetColor();
        Console.ForegroundColor = color;
        Console.Write(c);
        Console.ResetColor();
    }
}