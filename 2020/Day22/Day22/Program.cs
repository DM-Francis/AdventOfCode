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