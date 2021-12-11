using MathNet.Numerics.Statistics;

var data = File.ReadAllText("input.txt");

var numbers = data.Split(',').Select(x => double.Parse(x)).ToList();

double median = numbers.Median();
double mean = numbers.Mean();

double meanRounded = 484;// Math.Round(mean);

double fuelAtMedian = numbers.Sum(x => Math.Abs(x - median));
double fuelAtMean = numbers.Sum(x => CalcFuelExpensive(x, meanRounded));

Console.WriteLine($"Median: {median}");
Console.WriteLine($"Fuel at median: {fuelAtMedian}");

Console.WriteLine($"Mean: {mean}");
Console.WriteLine($"Rounded mean: {meanRounded}");
Console.WriteLine($"Fuel at mean: {fuelAtMean}");


static double CalcFuelExpensive(double number, double target)
{
    double diff = Math.Abs(number - target);
    return diff * (diff + 1) / 2;
}