namespace Day21;

public readonly record struct GameState(
    int Player1Position,
    int Player2Position,
    int Player1Score,
    int Player2Score,
    int NextPlayer)
{
    private const int WinThreshold = 1000;

    public int Priority => Player1Score + Player2Score;
    public bool IsComplete => Player1Score >= WinThreshold || Player2Score >= WinThreshold;
    
    public int? Winner
    {
        get
        {
            if (Player1Score >= WinThreshold)
                return 1;
            if (Player2Score >= WinThreshold)
                return 2;
            return null;
        }
    }

    public GameState MakeMove(int diceResult)
    {
        int newPosition;
        int newScore;
        switch (NextPlayer)
        {
            case 1:
                newPosition = MovePlayer(Player1Position, diceResult);
                newScore = Player1Score + newPosition;
                return new GameState(
                    newPosition,
                    Player2Position,
                    newScore,
                    Player2Score,
                    2);
            case 2:
                newPosition = MovePlayer(Player2Position, diceResult);
                newScore = Player2Score + newPosition;
                return new GameState(
                    Player1Position,
                    newPosition,
                    Player1Score,
                    newScore,
                    1);
            default:
                throw new ArgumentOutOfRangeException(nameof(NextPlayer));
        }
    }

    private static int MovePlayer(int position, int diceResult)
    {
        int newPosition = (position + diceResult) % 10;
        return newPosition == 0 ? 10 : newPosition;
    }
}