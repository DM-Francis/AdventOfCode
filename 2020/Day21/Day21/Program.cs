using Day21;

var input = File.ReadAllLines("input");
var exampleInput = """
                   mxmxvkd kfcds sqjhc nhms (contains dairy, fish)
                   trh fvjkl sbzzf mxmxvkd (contains dairy)
                   sqjhc fvjkl (contains soy)
                   sqjhc mxmxvkd sbzzf (contains fish)
                   """.Split(Environment.NewLine);

var foods = new List<Food>();
foreach (var line in input)
{
    const StringSplitOptions trimAndNonEmpty = StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries;
    var parts = line[..^1].Split(" (contains ");
    var ingredients = parts[0].Split(' ', trimAndNonEmpty);
    var allergens = parts[1].Split(", ", trimAndNonEmpty);
    var food = new Food(ingredients, allergens);
    foods.Add(food);
}

var allAllergens = foods.SelectMany(f => f.Allergens).Distinct().ToList();
var couldContainAllergen = new HashSet<string>();
var allergenCandidates = new Dictionary<string, HashSet<string>>();

foreach (var allergen in allAllergens)
{
    var candidates = FindCandidateIngredientsForAllergen(allergen, foods);
    allergenCandidates[allergen] = new HashSet<string>();
    foreach (var candidate in candidates)
    {
        couldContainAllergen.Add(candidate);
        allergenCandidates[allergen].Add(candidate);
    }
}

// Part 1
var allIngredients = foods.SelectMany(f => f.Ingredients).Distinct();
var definitelyNotAllergen = allIngredients.Where(i => !couldContainAllergen.Contains(i));

int appearanceCount = 0;
foreach (var ingredient in definitelyNotAllergen)
{
    appearanceCount += foods.Count(f => f.Ingredients.Contains(ingredient));
}

Console.WriteLine($"Appearance count for ingredients that are not an allergen: {appearanceCount}");

// Part 2
Console.WriteLine($"Allergen count: {allAllergens.Count}");
Console.WriteLine($"Could contain allergen count: {couldContainAllergen.Count}");

var finalAllergenAssignments = new Dictionary<string, string>();

while (finalAllergenAssignments.Count < allAllergens.Count)
{
    foreach (var (allergen, ingredients) in allergenCandidates.OrderBy(x => x.Value.Count).ToList())
    {
        if (ingredients.Count == 1)
        {
            var ingredient = ingredients.Single();
            finalAllergenAssignments[allergen] = ingredient;
            foreach (var item in allergenCandidates.Values)
            {
                item.Remove(ingredient);
            }
        }
    }
}


var orderedAllergenIngredients = finalAllergenAssignments.OrderBy(x => x.Key).Select(x => x.Value).ToList();
string finalList = string.Join(',', orderedAllergenIngredients);

Console.WriteLine($"Canonical dangerous ingredient list: {finalList}");

return;

static IEnumerable<string> FindCandidateIngredientsForAllergen(string allergen, IEnumerable<Food> foods)
{
    var candidates = new List<string>();
    foreach (var food in foods.Where(f => f.Allergens.Contains(allergen)))
    {
        if (candidates.Count == 0)
            candidates.AddRange(food.Ingredients);
        else
            candidates = candidates.Intersect(food.Ingredients).ToList();
    }

    return candidates;
}