namespace Forge.Diet.Domain.ValueObjects;

public sealed record Nutrition
{
    public decimal Calories { get; }
    public decimal Protein { get; }
    public decimal Carbohydrates { get; }
    public decimal Fat { get; }
    public decimal Fiber { get; }

    private Nutrition(
        decimal calories,
        decimal protein,
        decimal carbohydrates,
        decimal fat,
        decimal fiber)
    {
        Calories = calories;
        Protein = protein;
        Carbohydrates = carbohydrates;
        Fat = fat;
        Fiber = fiber;
    }

    public static Nutrition Create(
        decimal calories,
        decimal protein,
        decimal carbohydrates,
        decimal fat,
        decimal fiber)
    {
        if (calories < 0)
            throw new ArgumentOutOfRangeException(nameof(calories));

        if (protein < 0)
            throw new ArgumentOutOfRangeException(nameof(protein));

        if (carbohydrates < 0)
            throw new ArgumentOutOfRangeException(nameof(carbohydrates));

        if (fat < 0)
            throw new ArgumentOutOfRangeException(nameof(fat));

        if (fiber < 0)
            throw new ArgumentOutOfRangeException(nameof(fiber));

        return new Nutrition(
            calories,
            protein,
            carbohydrates,
            fat,
            fiber);
    }
}