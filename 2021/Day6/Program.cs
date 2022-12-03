using System.Numerics;

var fish = File.ReadAllLines("input.txt")[0]
    .Split(',')
    .Select(x => int.Parse(x))
    .ToList();

Console.WriteLine($"Fish count: {fish.Count}");

var fishState = (from f in fish
                   group f by f into g
                   select new { Timer = g.Key, Count = g.LongCount() }).ToDictionary(x => x.Timer, x => (BigInteger)x.Count);

for (int i = 0; i < 5000; i++)
{
    fishState = IterateDay(fishState);
    BigInteger totalCount = fishState.Values.Aggregate(BigInteger.Add);

    Console.WriteLine($"Day {i+1,3}    Fish count: {totalCount}");
}

static Dictionary<int, BigInteger> IterateDay(Dictionary<int, BigInteger> fishState)
{
    var nextFishState = new Dictionary<int, BigInteger>
    {
        {0,0},
        {1,0},
        {2,0},
        {3,0},
        {4,0},
        {5,0},
        {6,0},
        {7,0},
        {8,0}
    };

    foreach(int timerValue in fishState.Keys)
    {
        if (timerValue == 0)
        {
            nextFishState[6] += fishState[timerValue];
            nextFishState[8] += fishState[timerValue];
        }
        else
        {
            nextFishState[timerValue - 1] += fishState[timerValue];
        }
    }

    return nextFishState;
}