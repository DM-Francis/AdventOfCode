using Day20;

var exampleInput = """
                   ..#.#..#####.#.#.#.###.##.....###.##.#..###.####..#####..#....#..#..##..###..######.###...####..#..#####..##..#.#####...##.#.#..#.##..#.#......#.###.######.###.####...#.##.##..#..#..#####.....#.#....###..#.##......#.....#..#..#..##..#...##.######.####.####.#.#...#.......#..#.#.#...####.##.#......#..#...##.#.##..#...##.#.##..###.#......#.#.......#.#.#.####.###.##...#.....####.#..#..#.##.#....##..#.####....##...##..#...#......#.#.......#.......##..####..#...#.#.#...##..#.#..###..#####........#..####......#..#

                   #..#.
                   #....
                   ##..#
                   ..#..
                   ..###
                   """;

var input = File.ReadAllLines("input");
// var input = exampleInput.Split(Environment.NewLine);

string rawEnhancementAlgorithm = input[0];
var imageRaw = input[2..];

var enhancementMap = new Dictionary<int, bool>();
for (int i = 0; i < rawEnhancementAlgorithm.Length; i++)
{
    enhancementMap[i] = rawEnhancementAlgorithm[i] == '#';
}

var image = new Image(imageRaw);
Console.WriteLine("Initial image:");
RenderImage(image);

for (int i = 0; i < 50; i++)
{
    image.ApplyEnhancement(enhancementMap);
    Console.WriteLine($"Completed {i + 1} enhancements.");
}

Console.WriteLine("Final image:");
RenderImage(image);

int litPixels = image.Pixels.Count(p => p.Value);
Console.WriteLine($"Lit pixels: {litPixels}");

return;

static void RenderImage(Image image)
{
    for (int y = image.MinY; y <= image.MaxY; y++)
    {
        for (int x = image.MinX; x <= image.MaxX; x++)
        {
            Console.Write(image.GetValue(x, y) ? '#' : '.');
        }
        Console.WriteLine();
    }
}