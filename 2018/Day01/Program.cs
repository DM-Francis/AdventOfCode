var input = File.ReadAllLines("input.txt");

// Part 1
int frequency = 0;
foreach (var change in input)
{
    int frequencyChange = int.Parse(change);
    frequency += frequencyChange;
}

Console.WriteLine($"Final frequency: {frequency}");

// Part 2
int duplicateFrequency = GetFirstDuplicateFrequency(input);

Console.WriteLine($"First duplicate frequency: {duplicateFrequency}");

static int GetFirstDuplicateFrequency(string[] input)
{
    int frequency = 0;
    var allFrequencies = new HashSet<int> {frequency};

    while (true)
    {
        foreach (var change in input)
        {
            frequency += int.Parse(change);
            if (allFrequencies.Contains(frequency))
                return frequency;

            allFrequencies.Add(frequency);
        }
    }
}