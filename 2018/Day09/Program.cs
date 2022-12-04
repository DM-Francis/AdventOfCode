var input = File.ReadAllLines("input.txt")[0].Split(" ");

int playerCount = int.Parse(input[0]);
int maxMarbleScore = int.Parse(input[6]);

var playerScores = new long[playerCount];
var circle = new Circle();

int currentPlayer = 0;
int marbleValue = 1;
while (marbleValue <= maxMarbleScore * 100)
{
    if (marbleValue % 100000 == 0)
        Console.WriteLine(marbleValue);
    
    playerScores[currentPlayer] += circle.AddMarbleAndGetScore(marbleValue);
    currentPlayer = (currentPlayer + 1) % playerCount;
    marbleValue++;
}

long highestScore = playerScores.Max();

Console.WriteLine($"Winning player's score: {highestScore}");


public class Circle
{
    private readonly LinkedList<int> _marbles = new();
    private LinkedListNode<int> _currentMarble;

    public Circle()
    {
        var firstMarble = new LinkedListNode<int>(0);
        _marbles.AddFirst(firstMarble);
        _currentMarble = firstMarble;
    }
    
    public int AddMarbleAndGetScore(int marbleValue)
    {
        if (marbleValue > 0 && marbleValue % 23 == 0)
        {
            var marbleToRemove = GetAntiClockwise(_currentMarble, 7);
            int score = marbleValue + marbleToRemove.Value;
            _currentMarble = GetNextClockwise(marbleToRemove);
            _marbles.Remove(marbleToRemove);
            
            return score;
        }

        var newMarble = new LinkedListNode<int>(marbleValue);
        _marbles.AddAfter(GetNextClockwise(_currentMarble), newMarble);
        _currentMarble = newMarble;
        return 0;
    }

    private static LinkedListNode<int> GetNextClockwise(LinkedListNode<int> marble)
    {
        return marble.Next ?? marble.List!.First!;
    }

    private static LinkedListNode<int> GetNextAntiClockwise(LinkedListNode<int> marble)
    {
        return marble.Previous ?? marble.List!.Last!;
    }

    private static LinkedListNode<int> GetAntiClockwise(LinkedListNode<int> marble, int count)
    {
        for (int i = 0; i < count; i++)
        {
            marble = GetNextAntiClockwise(marble);
        }

        return marble;
    }
}