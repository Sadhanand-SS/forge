namespace Forge.Diet.Application.Ingredients.Queries.SearchIngredients;

public record SearchIngredientsQuery(string? Name, string? Brand, string? Cursor = null, int Limit = 20);
