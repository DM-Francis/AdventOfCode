using Day24;

var input = File.ReadAllLines("input.txt");
var exampleInput = """
                   #.######
                   #>>.<^<#
                   #.<..<<#
                   #>v.><>#
                   #<^v^^>#
                   ######.#
                   """.Split(Environment.NewLine);

var valley = GenerateValleyFromInput(input);
Renderer.RenderValley(valley);

var minTime1 = PathFinder.FindPathThroughValley(valley);
Console.WriteLine($"Minimum time to reach the finish: {minTime1}");
var minTime2 = PathFinder.FindPathThroughValleyBetweenPositions(valley, valley.Finish, valley.Start, minTime1);
Console.WriteLine($"Time back to start again: {minTime2}");
var minTime3 = PathFinder.FindPathThroughValleyBetweenPositions(valley, valley.Start, valley.Finish, minTime2);
Console.WriteLine($"Time back to finish again: {minTime3}");

return;



static Valley GenerateValleyFromInput(string[] strings)
{
    int width = strings[0].Length;
    int height = strings.Length;

    var valley = new Valley(width, height);

    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            char c = strings[y][x];
            switch (c)
            {
                case '#':
                    valley.Walls[x, y] = true;
                    break;
                case '^':
                    valley.Blizzards[x, y].Add(Direction.Up);
                    break;
                case 'v':
                    valley.Blizzards[x, y].Add(Direction.Down);
                    break;
                case '<':
                    valley.Blizzards[x, y].Add(Direction.Left);
                    break;
                case '>':
                    valley.Blizzards[x, y].Add(Direction.Right);
                    break;
                case '.':
                    if (y == 0)
                        valley.Start = new Position(x, y);
                    else if (y == height - 1)
                        valley.Finish = new Position(x, y);
                    break;
            }
        }
    }

    return valley;
}