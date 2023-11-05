using Day22;

var input = File.ReadAllText("input");
var exampleInput = """
                   Player 1:
                   9
                   2
                   6
                   3
                   1

                   Player 2:
                   5
                   8
                   4
                   7
                   10
                   """;

var parts = input.Split($"{Environment.NewLine}{Environment.NewLine}");

var player1Deck = ParsePlayersDeck(parts[0]);
var player2Deck = ParsePlayersDeck(parts[1]);

while (player1Deck.Count > 0 && player2Deck.Count > 0)
    PlayRound(player1Deck, player2Deck);

var winningDeck = player1Deck.Count > 0 ? player1Deck : player2Deck;

var winningScore = CalculateScore(winningDeck);

Console.WriteLine($"Winning score: {winningScore}");

// Part 2
player1Deck = ParsePlayersDeck(parts[0]);
player2Deck = ParsePlayersDeck(parts[1]);

var result = PlayRecursiveGame(player1Deck, player2Deck);

winningScore = CalculateScore(result.WinningDeck);

Console.WriteLine($"Winning score for recursive game: {winningScore}");


return;

static Queue<int> ParsePlayersDeck(string playerInput)
{
    var lines = playerInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
    var deck = new Queue<int>();
    for (int i = 1; i < lines.Length; i++)
    {
        deck.Enqueue(int.Parse(lines[i]));
    }

    return deck;
}

static void PlayRound(Queue<int> player1deck, Queue<int> player2deck)
{
    int played1 = player1deck.Dequeue();
    int played2 = player2deck.Dequeue();

    if (played1 > played2)
    {
        player1deck.Enqueue(played1);
        player1deck.Enqueue(played2);
        return;
    }

    if (played2 > played1)
    {
        player2deck.Enqueue(played2);
        player2deck.Enqueue(played1);
    }
}

static (Player Winner, Queue<int> WinningDeck) PlayRecursiveGame(Queue<int> player1Deck, Queue<int> player2Deck)
{
    Player? immediateGameWinner = null;
    var previousStates = new HashSet<DeckState> { new(player1Deck, player2Deck)};
    while (player1Deck.Count > 0 && player2Deck.Count > 0 && immediateGameWinner is null)
    {
        PlayRecursiveRound(player1Deck, player2Deck);
        var newState = new DeckState(player1Deck, player2Deck);
        if (previousStates.Contains(newState))
        {
            immediateGameWinner = Player.One;
            break;
        }
            
        previousStates.Add(newState);
    }

    Player winningPlayer;
    if (immediateGameWinner is not null)
        winningPlayer = immediateGameWinner.Value;
    else
        winningPlayer = player1Deck.Count > 0 ? Player.One : Player.Two;
    
    var winningDeck = winningPlayer == Player.One ? player1Deck : player2Deck;

    return (winningPlayer, winningDeck);
}

static void PlayRecursiveRound(Queue<int> player1Deck, Queue<int> player2Deck)
{
    int played1 = player1Deck.Dequeue();
    int played2 = player2Deck.Dequeue();

    bool player1HasEnoughCardsInDeck = player1Deck.Count >= played1;
    bool player2HasEnoughCardsInDeck = player2Deck.Count >= played2;

    Player winner;
    if (player1HasEnoughCardsInDeck && player2HasEnoughCardsInDeck)
    {
        var newPlayer1Deck = new Queue<int>(player1Deck.Take(played1));
        var newPlayer2Deck = new Queue<int>(player2Deck.Take(played2));
        var result = PlayRecursiveGame(newPlayer1Deck, newPlayer2Deck);
        winner = result.Winner;
    }
    else if (played1 > played2)
    {
        winner = Player.One;
    }
    else if (played2 > played1)
    {
        winner = Player.Two;
    }
    else
    {
        throw new InvalidOperationException("Both played cards were the same value, this should not be possible");
    }

    if (winner == Player.One)
    {
        player1Deck.Enqueue(played1);
        player1Deck.Enqueue(played2);
    }
    else
    {
        player2Deck.Enqueue(played2);
        player2Deck.Enqueue(played1);
    }
}

static int CalculateScore(Queue<int> deck)
{
    int score = 0;
    var cards = deck.ToList();
    for (int i = 0; i < cards.Count; i++)
    {
        score += (cards.Count - i) * cards[i];
    }

    return score;
}

public enum Player { One, Two }