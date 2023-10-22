namespace Day24;

public static class Renderer
{
    public static void RenderValley(Valley valley, Position withPosition = default)
    {
        for (int y = 0; y < valley.Height; y++)
        {
            for (int x = 0; x < valley.Width; x++)
            {
                if (withPosition != default && withPosition.X == x && withPosition.Y == y)
                {
                    Console.Write('E');
                    continue;
                }

                var tile = valley.GetTileAt(x, y);
                Console.Write(tile switch
                {
                    Tile.Empty => '.',
                    Tile.Wall => '#',
                    Tile.Blizzard => RenderBlizzardTile(x,y,valley),
                    Tile.Start => 'S',
                    Tile.Finish => 'F',
                    _ => throw new ArgumentOutOfRangeException()
                });
            }
        
            Console.WriteLine();
        }
    }

    private static char RenderBlizzardTile(int x, int y, Valley valley)
    {
        int count = valley.Blizzards[x, y].Count;
        return count switch
        {
            0 => '.',
            1 => valley.Blizzards[x, y][0] switch
            {
                Direction.Up => '^',
                Direction.Down => 'v',
                Direction.Left => '<',
                Direction.Right => '>',
                _ => throw new ArgumentOutOfRangeException()
            },
            >= 10 => '%',
            _ => count.ToString()[0]
        };
    }
}