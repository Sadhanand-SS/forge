using System;

namespace Forge.Diet.Domain.Entities;

public class DailyMealMealItem
{
    public Guid Id { get; private set; }

    public Guid DailyMealId { get; private set; }

    public DailyMeal DailyMeal { get; private set; }

    public Guid MealItemId { get; private set; }

    public MealItem MealItem { get; private set; }

    public decimal Servings { get; private set; }

    private DailyMealMealItem()
    {
        DailyMeal = null!;
        MealItem = null!;
    }

    public DailyMealMealItem(Guid id, MealItem mealItem, decimal servings)
    {
        if (servings <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(servings), "Servings must be greater than zero.");
        }

        Id = id;
        DailyMeal = null!;
        MealItem = mealItem ?? throw new ArgumentNullException(nameof(mealItem));
        MealItemId = mealItem.Id;
        Servings = servings;
    }

    public void UpdateServings(decimal servings)
    {
        if (servings <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(servings), "Servings must be greater than zero.");
        }

        Servings = servings;
    }
}
