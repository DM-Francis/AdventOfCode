namespace Day21;

public class Food
{
    public HashSet<string> Ingredients { get; } = new();
    public HashSet<string> Allergens { get; } = new();

    public Food(IEnumerable<string> ingredients, IEnumerable<string> allergens)
    {
        foreach (var ingredient in ingredients)
            Ingredients.Add(ingredient);

        foreach (var allergen in allergens)
            Allergens.Add(allergen);
    }
}