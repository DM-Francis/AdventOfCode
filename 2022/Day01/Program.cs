string input = File.ReadAllText("input.txt");

string[] splitByElf = input.Split("\n\n");

var caloriesPerElf = splitByElf.Select(elfFood =>
{
    var items = elfFood.Split("\n", StringSplitOptions.RemoveEmptyEntries);
    return items.Sum(int.Parse);
}).ToList();

int maxCalories = caloriesPerElf.Max();
var top3ElfCalories = caloriesPerElf.OrderByDescending(x => x).Take(3).ToList();

Console.WriteLine($"Max calories: {maxCalories}");
Console.WriteLine($"Top 3 calories: {string.Join(',', top3ElfCalories)}");
Console.WriteLine($"Top 3 total calories: {top3ElfCalories.Sum()}");