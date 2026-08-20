using System;
using System.Collections.Generic;
using System.Linq;
using Forge.Diet.Domain.ValueObjects;

namespace Forge.Diet.Domain.Entities;

public class DailyMeal
{
    public Guid Id { get; private set; }

    public DateOnly Date { get; private set; }

    public Guid MealId { get; private set; }

    public Meal Meal { get; private set; }

    public bool IsSkipped { get; private set; }

    public ICollection<DailyMealMealItem> MealItems { get; private set; }

    private DailyMeal()
    {
        Meal = null!;
        MealItems = new List<DailyMealMealItem>();
    }

    public DailyMeal(Guid id, DateOnly date, Meal meal)
    {
        Id = id;
        Date = date;
        Meal = meal ?? throw new ArgumentNullException(nameof(meal));
        MealId = meal.Id;
        MealItems = new List<DailyMealMealItem>();
    }

    /// <summary>
    /// Creates a DailyMeal using only the FK. Use this when the Meal entity was loaded
    /// with AsNoTracking() to prevent EF re-attaching and inserting the Meal row again.
    /// </summary>
    public DailyMeal(Guid id, DateOnly date, Guid mealId)
    {
        Id = id;
        Date = date;
        MealId = mealId;
        Meal = null!;
        MealItems = new List<DailyMealMealItem>();
    }

    public void SetSkipped(bool isSkipped)
    {
        IsSkipped = isSkipped;
    }

    public DailyMealMealItem AddMealItem(MealItem mealItem, decimal servings)
    {
        if (mealItem == null)
        {
            throw new ArgumentNullException(nameof(mealItem));
        }

        var dailyMealMealItem = new DailyMealMealItem(Guid.NewGuid(), mealItem, servings);
        MealItems.Add(dailyMealMealItem);
        return dailyMealMealItem;
    }

    public void RemoveMealItem(Guid dailyMealMealItemId)
    {
        var mealItem = MealItems.FirstOrDefault(item => item.Id == dailyMealMealItemId);
        if (mealItem != null)
        {
            MealItems.Remove(mealItem);
        }
    }

    public Nutrition GetTotalNutrition()
    {
        if (IsSkipped)
        {
            return Nutrition.Create(0, 0, 0, 0, 0);
        }

        decimal calories = 0;
        decimal protein = 0;
        decimal carbohydrates = 0;
        decimal fat = 0;
        decimal fiber = 0;

        foreach (var mealMealItem in MealItems)
        {
            if (mealMealItem.MealItem == null)
            {
                continue;
            }

            var itemNutrition = mealMealItem.GetTotalNutrition();
            calories += itemNutrition.Calories * mealMealItem.Servings;
            protein += itemNutrition.Protein * mealMealItem.Servings;
            carbohydrates += itemNutrition.Carbohydrates * mealMealItem.Servings;
            fat += itemNutrition.Fat * mealMealItem.Servings;
            fiber += itemNutrition.Fiber * mealMealItem.Servings;
        }

        return Nutrition.Create(calories, protein, carbohydrates, fat, fiber);
    }
}
