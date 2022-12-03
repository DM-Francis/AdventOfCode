using Day4;

var data = File.ReadAllLines("input.txt");

List<int> drawnNumbers = data[0].Split(',').Select(x => int.Parse(x)).ToList();

var boardData = GetBoardData(data);
var boards1 = boardData.Select(x => new Board(x)).ToList();
var boards2 = boardData.Select(x => new Board(x)).ToList();

Console.WriteLine("Running part one...");
PartOne(drawnNumbers, boards1);
Console.WriteLine("Running part two...");
PartTwo(drawnNumbers, boards2);

static void PartOne(List<int> drawnNumbers, List<Board> boards)
{
    foreach (int number in drawnNumbers)
    {
        Console.WriteLine($"Calling number {number}");
        MarkAllBoards(boards, number);

        if (boards.Any(b => b.HasWon()))
        {
            Console.WriteLine("A board has won");
            var winningBoard = boards.Single(b => b.HasWon());

            Console.WriteLine("The winning board is:");
            winningBoard.WriteToConsole();

            int finalScore = winningBoard.GetAllUnmarkedNumbers().Sum() * number;
            Console.WriteLine($"Final score: {finalScore}");
            break;
        }
    }
}

static void PartTwo(List<int> drawnNumbers, List<Board> boards)
{
    foreach (int number in drawnNumbers)
    {
        Console.WriteLine($"Calling number {number}");
        MarkAllBoards(boards, number);

        if (boards.Any(b => b.HasWon()))
        {
            if (boards.Count == 1)
            {
                var winningBoard = boards[0];
                Console.WriteLine("Final board has won:");
                winningBoard.WriteToConsole();

                int finalScore = winningBoard.GetAllUnmarkedNumbers().Sum() * number;
                Console.WriteLine($"Final score: {finalScore}");
                break;
            }

            Console.WriteLine("Boards have won, removing winning boards");
            boards.RemoveAll(b => b.HasWon());
        }
    }
}

static IEnumerable<List<string>> GetBoardData(IList<string> inputData)
{
    var currentBoard = new List<string>();

    for (int i = 2; i < inputData.Count; i++)
    {
        if (!string.IsNullOrEmpty(inputData[i]))
        {
            currentBoard.Add(inputData[i]);
        }
        else
        {
            yield return currentBoard;
            currentBoard = new List<string>();
        }
    }
}

static void MarkAllBoards(IEnumerable<Board> boards, int number)
{
    foreach (var board in boards)
    {
        board.Mark(number);
    }
}