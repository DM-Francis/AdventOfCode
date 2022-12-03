var input = File.ReadAllLines("input.txt");

// Part 1
int totalScore = 0;
foreach (var round in input)
{
    var split = round.Split(" ").Select(x => x[0]).ToList();
    var opponent = split[0];
    var played = split[1];

    totalScore += ScoreForRound(ShapeFromOpponent(opponent), ShapeFromPlayed(played));
}

Console.WriteLine($"Total score (part 1): {totalScore}");

// Part 2
int totalScore2 = 0;
foreach (var round in input)
{
    var split = round.Split(" ").Select(x => x[0]).ToList();
    var opponentPlayed = ShapeFromOpponent(split[0]);
    var desiredResult = ResultFromChar(split[1]);
    var toPlay = RequiredShapeForResult(opponentPlayed, desiredResult);
    totalScore2 += ScoreForRound(opponentPlayed, toPlay);
}

Console.WriteLine($"Total score (part 2): {totalScore2}");

static int ScoreForRound(Shape opponent, Shape played)
{
    const int win = 6;
    const int draw = 3;
    const int lose = 0;
    var score = new Dictionary<Shape, int> {{Shape.Rock, 1}, {Shape.Paper, 2}, {Shape.Scissors, 3}};

    if (opponent == played)
        return draw + score[played];
    if (opponent == Shape.Rock && played == Shape.Paper ||
        opponent == Shape.Paper && played == Shape.Scissors ||
        opponent == Shape.Scissors && played == Shape.Rock)
        return win + score[played];

    return lose + score[played];
}

static Shape RequiredShapeForResult(Shape opponent, Result desiredResult)
{
    switch (desiredResult)
    {
        case Result.Draw:
            return opponent;
        case Result.Win when opponent == Shape.Rock:
        case Result.Lose when opponent == Shape.Scissors:
            return Shape.Paper;
        case Result.Win when opponent == Shape.Paper:
        case Result.Lose when opponent == Shape.Rock:
            return Shape.Scissors;
        case Result.Win when opponent == Shape.Scissors:
        case Result.Lose when opponent == Shape.Paper:
            return Shape.Rock;
        default:
            throw new ArgumentOutOfRangeException(nameof(desiredResult), desiredResult, "Unknown result value");
    }
}

static Shape ShapeFromOpponent(char shapeChar) => (Shape)(shapeChar - 65);
static Shape ShapeFromPlayed(char shapeChar) => (Shape)(shapeChar - 88);

static Result ResultFromChar(char result) => (Result)(result - 88);

public enum Shape
{
    Rock = 0,
    Paper = 1,
    Scissors = 2
}

public enum Result
{
    Lose = 0,
    Draw = 1,
    Win = 2
}