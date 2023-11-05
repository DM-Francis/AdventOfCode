using Day23;

// var input = "389125467"; // Example input
var input = "459672813";

var circle = new Circle(input);

for (int i = 0; i < 100; i++)
{
    circle.Move3AtCurrent();
}

var finalValuesAfter1 = circle.GetLabelsAfter1();
string finalString = string.Join("", finalValuesAfter1);
Console.WriteLine($"Final labels after 1: {finalString}");

// Part 2
circle = new Circle(input, 1_000_000);

for (int i = 0; i < 10_000_000; i++)
{
    if (i % 100000 == 0)
        Console.WriteLine($"At move {i}");
    circle.Move3AtCurrent();
}

var next2After1 = circle.Get2LabelsClockwiseOf1();

long answer = (long)next2After1.a * next2After1.b;

Console.WriteLine($"Answer: {answer}");
