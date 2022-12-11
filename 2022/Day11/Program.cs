using Day11;

var monkeys = new List<Monkey>
{
    new (new Worry[] {62, 92, 50, 63, 62, 93, 73, 50},
        old => old * 7,
        worry => worry.Mod2 == 0 ? 7 : 1),
    new (new Worry[] {51, 97, 74, 84, 99},
        old => old + 3,
        worry => worry.Mod7 == 0 ? 2 : 4),
    new (new Worry[] {98, 86, 62, 76, 51, 81, 95},
        old => old + 4,
        worry => worry.Mod13 == 0 ? 5 : 4),
    new (new Worry[] {53, 95, 50, 85, 83, 72},
        old => old + 5,
        worry => worry.Mod19 == 0 ? 6 : 0),
    new (new Worry[] {59, 60, 63, 71},
        old => old * 5,
        worry => worry.Mod11 == 0 ? 5 : 3),
    new (new Worry[] {92, 65},
        old => old * old,
        worry => worry.Mod5 == 0 ? 6 : 3),
    new (new Worry[] {78},
        old => old + 8,
        worry => worry.Mod3 == 0 ? 0 : 7),
    new (new Worry[] {84, 93, 54},
        old => old + 1,
        worry => worry.Mod17 == 0 ? 2 : 1),
};

const int n = 10000;
RunNRounds(n, monkeys);
long monkeyBusiness = GetMonkeyBusinessLevel(monkeys);
Console.WriteLine($"Level of monkey business after {n} rounds: {monkeyBusiness}");

static void RunNRounds(int n, List<Monkey> monkeys)
{
    for (int i = 0; i < n; i++)
    {
        CompleteSingleRound(monkeys);
    }
}

static void CompleteSingleRound(List<Monkey> monkeys)
{
    foreach (var monkey in monkeys)
    {
        while (monkey.Items.Count > 0)
        {
            ProcessNextItemForMonkey(monkey, monkeys);
        }
    }
}

static void ProcessNextItemForMonkey(Monkey currentMonkey, IReadOnlyList<Monkey> monkeys)
{
    var item = currentMonkey.Items.Dequeue();
    var worry = currentMonkey.Operation(item);
    // int worryFinal = worryProcessed / 3;
    int nextMonkeyIndex = currentMonkey.Test(worry);

    currentMonkey.InspectionCount++;
    
    monkeys[nextMonkeyIndex].Items.Enqueue(worry);
}

static long GetMonkeyBusinessLevel(IEnumerable<Monkey> monkeys)
{
    var top2Monkeys = monkeys.OrderByDescending(m => m.InspectionCount).Take(2).ToList();
    long monkeyBusiness = top2Monkeys[0].InspectionCount * top2Monkeys[1].InspectionCount;
    return monkeyBusiness;
}