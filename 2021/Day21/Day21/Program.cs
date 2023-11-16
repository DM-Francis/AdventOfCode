using System.Numerics;
using Day21;

int player1Position = 3;
int player2Position = 4;

int player1Score = 0;
int player2Score = 0;

int nextRoll = 1;
int rollCount = 0;

while (true)
{
    player1Position = MovePlayer(player1Position);
    player1Score += player1Position;
    if (player1Score >= 1000)
        break;

    player2Position = MovePlayer(player2Position);
    player2Score += player2Position;
    if (player2Score >= 1000)
        break;
}

int losingScore = Math.Min(player1Score, player2Score);
Console.WriteLine($"Points times roll count: {losingScore * rollCount}");

// Part Two
player1Position = 4;
player2Position = 8;

player1Score = 0;
player2Score = 0;

var newUniversesPerResult = new Dictionary<int, int>
{
    [3] = 1,
    [4] = 3,
    [5] = 6,
    [6] = 7,
    [7] = 6,
    [8] = 3,
    [9] = 1,
};

var universesPerState = new Dictionary<GameState, BigInteger>();
var initialState = new GameState(player1Position, player2Position, player1Score, player2Score, 1);
universesPerState[initialState] = 1;

var queue = new PriorityQueue<GameState, int>();
queue.Enqueue(initialState, initialState.Priority);

int currentPriority = 0;
while (queue.Count > 0)
{
    var gameState = queue.Dequeue();
    if (gameState.IsComplete)
        continue;

    if (gameState.Priority > currentPriority)
    {
        currentPriority = gameState.Priority;
        Console.WriteLine($"Current priority: {currentPriority}");
    }
    
    var startingUniverseCount = universesPerState[gameState];
    foreach (var (diceResult, newUniverseCount) in newUniversesPerResult)
    {
        var newState = gameState.MakeMove(diceResult);
        var totalNewUniverses = startingUniverseCount * newUniverseCount;
        if (universesPerState.ContainsKey(newState))
        {
            universesPerState[newState] += totalNewUniverses;
        }
        else
        {
            universesPerState[newState] = totalNewUniverses;
            queue.Enqueue(newState, newState.Priority);
        }
    }

    universesPerState.Remove(gameState);
}

var player1WinCount = universesPerState.Where(x => x.Key.Winner == 1).Aggregate((BigInteger)0, (s, x) => s + x.Value);
var player2WinCount = universesPerState.Where(x => x.Key.Winner == 2).Aggregate((BigInteger)0, (s, x) => s + x.Value);

Console.WriteLine($"Player 1 winning universes: {player1WinCount}");
Console.WriteLine($"Player 2 winning universes: {player2WinCount}");


return;

int MovePlayer(int currentPosition)
{
    int rolls = RollDie() + RollDie() + RollDie();
    int newPosition = currentPosition + rolls;
    newPosition %= 10;
    return newPosition == 0 ? 10 : newPosition;
}

int RollDie()
{
    var roll = nextRoll;
    rollCount++;
    nextRoll++;
    if (nextRoll > 100)
        nextRoll = 1;
    return roll;
}
