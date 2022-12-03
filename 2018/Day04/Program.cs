using System.Globalization;

var input = File.ReadLines("input.txt").OrderBy(x => x).ToList();

var minutesAsleepPerGuard = new Dictionary<int, Dictionary<int, int>>();

int currentGuard = 0;
DateTime sleepStart = default;
foreach (string line in input)
{
    var timestamp = GetTimestamp(line);
    var message = line[19..];
    if (message.StartsWith('G'))
    {
        currentGuard = GetGuardId(message);
        minutesAsleepPerGuard.TryAdd(currentGuard, new Dictionary<int, int>());
    }
    else if (message.StartsWith('f'))
        sleepStart = timestamp;
    else if (message.StartsWith('w'))
    {
        for (int m = sleepStart.Minute; m < timestamp.Minute; m++)
        {
            if (minutesAsleepPerGuard[currentGuard].TryGetValue(m, out int minutesAsleep))
                minutesAsleepPerGuard[currentGuard][m] = minutesAsleep + 1;
            else
                minutesAsleepPerGuard[currentGuard][m] = 1;
        }
    }
}

// Part 1
int guardWithMostMinutesAsleep = minutesAsleepPerGuard.MaxBy(kv => kv.Value.Sum(x => x.Value)).Key;
int minuteWhereGuardIsAsleepMost = minutesAsleepPerGuard[guardWithMostMinutesAsleep].MaxBy(x => x.Value).Key;

Console.WriteLine($"Guard with most minutes asleep: {guardWithMostMinutesAsleep}");
Console.WriteLine($"Minute they are asleep most: {minuteWhereGuardIsAsleepMost}");
Console.WriteLine($"Product: {guardWithMostMinutesAsleep * minuteWhereGuardIsAsleepMost}");

// Part 2
int guardMostFrequentlyAsleepOnSameMinute = minutesAsleepPerGuard.MaxBy(g => g.Value.DefaultIfEmpty().Max(m => m.Value)).Key;
minuteWhereGuardIsAsleepMost = minutesAsleepPerGuard[guardMostFrequentlyAsleepOnSameMinute].MaxBy(x => x.Value).Key;

Console.WriteLine($"Guard asleep on the same minute most: {guardMostFrequentlyAsleepOnSameMinute}");
Console.WriteLine($"Minute they are asleep most: {minuteWhereGuardIsAsleepMost}");
Console.WriteLine($"Product: {guardMostFrequentlyAsleepOnSameMinute * minuteWhereGuardIsAsleepMost}");

static int GetGuardId(string message)
{
    return int.Parse(message.Split(' ')[1][1..]);
}

static DateTime GetTimestamp(string line)
{
    string timestampString = line[1..17];
    return DateTime.ParseExact(timestampString, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
}