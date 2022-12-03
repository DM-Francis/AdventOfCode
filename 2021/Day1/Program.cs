PartOne();
PartTwo();


void PartOne()
{
    var data = File.ReadAllLines("input.txt");

    int? previousMeasurement = null;
    int numberOfIncreases = 0;

    foreach (string line in data)
    {
        int thisMeasurement = int.Parse(line);
        if (thisMeasurement > previousMeasurement)
            numberOfIncreases++;

        previousMeasurement = thisMeasurement;
    }

    Console.WriteLine($"Number of increases: {numberOfIncreases}");
}

void PartTwo()
{
    var data = File.ReadAllLines("input.txt");

    int? previousMeasurement = null;
    int numberOfIncreases = 0;

    for (int i = 0; i < data.Length - 2; i++)
    {
        int thisMeasurement = int.Parse(data[i]) + int.Parse(data[i + 1]) + int.Parse(data[i + 2]);
        if (thisMeasurement > previousMeasurement)
            numberOfIncreases++;

        previousMeasurement = thisMeasurement;
    }

    Console.WriteLine($"Number of sliding window increases: {numberOfIncreases}");
}