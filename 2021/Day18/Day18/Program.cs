using Day18;

var input = File.ReadAllLines("input.txt");

var snailfishNumbers = input.Select(SnailfishNumber.Parse).ToList();
Console.ReadLine();